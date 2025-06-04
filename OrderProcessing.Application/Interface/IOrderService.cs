using OrderProcessing.Domain.Entity;

namespace OrderProcessing.Application.Interface
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(Order order);
    }
}
