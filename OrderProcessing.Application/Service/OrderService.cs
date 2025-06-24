using Newtonsoft.Json;
using OrderProcessing.Application.DTO;
using OrderProcessing.Application.Interface;
using OrderProcessing.Domain.Entity;
using StackExchange.Redis;
using Order = OrderProcessing.Domain.Entity.Order;

namespace OrderProcessing.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly IRedisService _redis;

        public OrderService(IOrderRepository orderRepository, IRabbitMqService rabbitMqService, IRedisService redisService)
        {
            _orderRepository = orderRepository;
            _rabbitMqService = rabbitMqService;
            _redis = redisService;
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

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public Task<bool> UpdateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
