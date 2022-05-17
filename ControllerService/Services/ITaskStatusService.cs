using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ControllerService.Services
{
    public enum ImageStatus
    { 
        Pending, 
        Started, 
        Success, 
        Failure
    }
    
    public interface ITaskStatusService
    {
        ImageStatus GetStatus(int taskId, IModel channel);
    }
}