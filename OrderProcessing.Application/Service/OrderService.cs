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
    }
}
