# DotnetServerImageClassification

.Net сервер для классификации картинок. Модель для распознавания - **inception_v3**.

Стек технологий: ASP.NET, RabbitMQ, Redis, Pytorch

Эндпоинты API:

POST /api/upload
Загрузить картинку (в формате .jpg) и поставить в очередь задачу инференса нейросети. Возвращает task_id (id задачи в очереди) и file_id (id файла загруженной картинки в хранилище).

GET /api/{task_id}/status
Узнать статус задачи: PENDING, STARTED, SUCCESS, FAILURE.

GET /api/{task_id}/download
Скачать результат при условии, что задача выполнена успешно. Результат может представлять из себя строку или file_id.

Сначала запускаем произвольное число воркеров вручную `python Worker/worker.py`\
Затем запускаем сам сервер.