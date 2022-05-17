using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RabbitMQ.Client;

namespace ControllerService.Services
{
    public struct InfoUploaded
    {
        public InfoUploaded(string taskId, string fileId)
        {
            TaskId = taskId;
            FileId = fileId;
        }

        public String TaskId { get; set; }
        public String FileId { get; set; }
    }
    public interface IUploadService
    {
        Task<InfoUploaded> Upload(IFormFile image, IModel channel);
    }
}