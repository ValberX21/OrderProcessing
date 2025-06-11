using OrderProcessing.Domain.Entity;

namespace OrderProcessing.Application.Interface
{
    public interface IRabbitMqService
    {
        Task SendMessage(OrderDto message);
    }
}