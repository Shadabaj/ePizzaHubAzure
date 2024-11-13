using System.Threading.Tasks;

namespace ePizzaHub.UI.Interfaces
{
    public interface IQueueService
    {
        Task SendMessageAsync<T>(T serviceBusMessage, string queueName);
    }
}