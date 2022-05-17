using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ControllerService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace ControllerService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly UploadService _uploadService = new UploadService();
        private readonly ITaskStatusService _getStatusService = new TaskStatusService();
        private readonly ITaskDownloaderService _downloadResultService = new TaskDownloaderService();
        
        private static string RABBIT_MQ_HOST = "localhost";
        private ConnectionFactory _rabbitConnection = new ConnectionFactory() {HostName = RABBIT_MQ_HOST};
        private IConnection _connection;
        private IModel _channel;

        public ApiController()
        {
            _connection = _rabbitConnection.CreateConnection();
            _channel = _connection.CreateModel();
        }

        [HttpPost("upload")]
        public ActionResult<InfoUploaded> Upload(IFormFile image)
        {
            if (image is null)
            {
                return new InfoUploaded("error", "error");
            }
            var answer = _uploadService.Upload(image, _channel).Result;
            return answer;
        }

        [HttpGet("status")]
        public ActionResult<string> Status(int taskId)
        {
            var status = _getStatusService.GetStatus(taskId, _channel);
            return status switch
            {
                ImageStatus.Pending => "PENDING",
                ImageStatus.Started => "STARTED",
                ImageStatus.Success => "SUCCESS",
                ImageStatus.Failure => "FAILURE",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        [HttpGet("download")]
        public ActionResult<String> Download(int taskId)
        {
            var result = _downloadResultService.Download(_uploadService.getPath(taskId), _channel);
            return result;
        }
    }
}