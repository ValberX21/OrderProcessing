using OrderProcessing.Domain.Entity;

namespace OrderProcessing.Application.Interface
{
    public interface IRabbitMqService
    {
        Task SendMessage(string queueName, OrderDto message);
    }
}