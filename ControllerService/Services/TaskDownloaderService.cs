using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ControllerService.Services
{
    public class TaskDownloaderService : ITaskDownloaderService
    {
        public String Download(String imagePath, IModel channel)
        {
            TaskStatusService statusService = new TaskStatusService();

            var status = statusService.GetStatus(taskId, channel);

            if (status != ImageStatus.Success)
            {
                return "failure";
            }
            
            
        }
    }
}