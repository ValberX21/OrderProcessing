using OrderProcessing.Application.Interface;
using OrderProcessing.Domain.Entity;
using OrderProcessing.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> CreateOrderAsync(Order order)
        {
            order.CreatedAt = DateTime.Now;           

            order.TotalAmount = order.Items.Sum(item => item.Quantity * item.UnitPrice);

           return await _orderRepository.AddAsync(order);

        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public Task<bool> DeleteOrderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> GetOrderByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
