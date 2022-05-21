import pika
import redis

import torch
from torchvision import datasets, transforms as T
import torchvision.models as models

from PIL import Image

import os

images_path = "Images/"
result_path = "Result/"

r = redis.Redis()

transform = T.Compose([
    T.Resize(299),
    T.CenterCrop(299),
    T.ToTensor(),
    T.Normalize(mean=[0.485, 0.456, 0.406], std=[0.229, 0.224, 0.225]),
])

inception = models.inception_v3(pretrained=True)
inception.eval()

with open("imagenet_classes.txt", "r") as f:
    categories = [s.strip() for s in f.readlines()]


def callback(ch, method, properties, body):
    r.mset({body: "STARTED"})
    ch.basic_ack(delivery_tag=method.delivery_tag)

    try:
        image = transform(Image.open(images_path + body + ".jpg"))
        output = inception(image.unsqueeze(dim=0))
        probabilities = torch.nn.functional.softmax(output[0], dim=0)

        top5_prob, top5_catid = torch.topk(probabilities, 5)

        result = ""
        for i in range(top5_prob.size(0)):
            result = f"Class: {categories[top5_catid[i]]},\t Class probability: {top5_prob[i].item()}\n"

        with open(result_path + body + ".txt", "w") as f:
            f.write(result)

        r.mset({body: "SUCCESS"})

    except:
        r.mset({body: "FAILURE"})


if __name__ == '__main__':
    connection = pika.BlockingConnection(
        pika.ConnectionParameters(host='localhost'))
    channel = connection.channel()

    channel.queue_declare(queue='task_queue', durable=True)
    channel.basic_qos(prefetch_count=1)
    channel.basic_consume(queue='task_queue', on_message_callback=callback)

    channel.start_consuming()
