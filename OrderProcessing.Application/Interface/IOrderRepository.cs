using OrderProcessing.Domain.Entity;

namespace OrderProcessing.Application.Interface
{
    public interface IOrderRepository
    {
        Task<OrderDto> AddAsync(Order order);
        Task<IEnumerable<OrderDto>> GetAllAsync();
    }
}
