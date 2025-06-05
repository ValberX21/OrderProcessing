using OrderProcessing.Domain.Entity;

namespace OrderProcessing.Application.Interface
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(Order order);
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
    }
}
