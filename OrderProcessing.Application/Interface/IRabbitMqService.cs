using OrderProcessing.Domain.Entity;

namespace OrderProcessing.Application.Interface
{
    public interface IRabbitMqService
    {
        Task SendMessageAsync(string queueName,OrderDto message);
    }
}
