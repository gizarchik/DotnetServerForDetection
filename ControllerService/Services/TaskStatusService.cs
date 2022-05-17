using System;
using System.Threading.Tasks;
using ControllerService.Helpers;
using RabbitMQ.Client;

namespace ControllerService.Services
{
    public class TaskStatusService : ITaskStatusService
    {
        public ImageStatus GetStatus(int taskId, IModel channel)
        {
            var database = RedisConnectorHelper.Connection.GetDatabase();
            var status = database.StringGet(taskId.ToString()).ToString();

            if (status == "PENDING")
            {
                return ImageStatus.Pending;
            } else if (status == "STARTED")
            {
                return ImageStatus.Started;
            } else if (status == "SUCCESS")
            {
                return ImageStatus.Success;
            } else if (status == "FAILURE")
            {
                return ImageStatus.Failure;
            }
            else
            {
                throw new Exception("Status from Redis invalid");
            }
        }
    }
}