using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ControllerService.Services
{
    public class TaskDownloaderService : ITaskDownloaderService
    {
        public string Download(string imagePath, IModel channel, int taskId)
        {
            TaskStatusService statusService = new TaskStatusService();

            var status = statusService.GetStatus(taskId, channel);

            if (status != ImageStatus.Success)
            {
                return "failure";
            }

            String content = File.ReadAllText("Result/" + taskId + ".txt");
            return content;
        }
    }
}