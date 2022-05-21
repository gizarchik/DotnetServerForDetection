using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RabbitMQ.Client;

namespace ControllerService.Services
{
    public class UploadService : IUploadService
    {
        private Dictionary<Int64, String> idToImages = new Dictionary<Int64, string>();
        private String STORAGE_PATH = "Images/";
        private static Int64 ID = 0;

        public String getPath(Int64 taskId)
        {
            return idToImages[taskId];
        }
        public async Task<InfoUploaded> Upload(IFormFile image, IModel channel)
        {
            String savedFileName = ID + ".jpg";
            String filePath = Path.Combine(STORAGE_PATH, savedFileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create)) {
                await image.CopyToAsync(fileStream);
            }

            var savedId = ID;
            ID += 1;
            // enqueue
            channel.BasicPublish(exchange: "",
                routingKey: "task_queue",
                basicProperties: channel.CreateBasicProperties(),
                body: Encoding.UTF8.GetBytes(ID.ToString())
                );
            idToImages[savedId] = savedFileName;
            return new InfoUploaded(savedId.ToString(), savedFileName);
        }
    }
}