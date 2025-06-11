using OrderProcessing.Application.DTO;
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
        private readonly IRabbitMqService _rabbitMqService;

        public OrderService(IOrderRepository orderRepository, IRabbitMqService rabbitMqService)
        {
            _orderRepository = orderRepository;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<OrderDto> CreateOrderAsync(Order order)
        {
            order.CreatedAt = DateTime.Now;           

            order.TotalAmount = order.Items.Sum(item => item.Quantity * item.UnitPrice);
            var orderDto = FromOrder(order);
            await _rabbitMqService.SendMessage("order-queue",orderDto);
            return await _orderRepository.AddAsync(order);

        }

        internal static OrderDto FromOrder(Order order)
        {
            if (order == null)
            {
                return null; // Or throw an ArgumentNullException, depending on desired behavior
            }

            return new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CreatedAt = order.CreatedAt,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                // Map the nested collection of items
                Items = order.Items? // Use null-conditional operator in case Items is null
                              .Select(item => new OrderItemDto
                              {
                                  ProductId = item.ProductId,
                                  ProductName = item.ProductName,
                                  Quantity = item.Quantity,
                                  UnitPrice = item.UnitPrice
                              })
                              .ToList() ?? new List<OrderItemDto>() // Ensure Items is never null
            };
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
