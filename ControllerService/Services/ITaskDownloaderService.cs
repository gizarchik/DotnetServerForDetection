using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ControllerService.Services
{
    public interface ITaskDownloaderService
    {
        string Download(string imagePath, IModel channel);
    }
}