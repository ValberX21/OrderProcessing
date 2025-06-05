using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.DTO;
using OrderProcessing.Application.Interface;
using OrderProcessing.Domain.Entity;
using OrderProcessing.Domain.Enum;
using OrderProcessing.Infrastructure.Data;

namespace OrderProcessing.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderDto> AddAsync(Order order)
        {
            try
            {
                _dbContext.Add(order);
                await _dbContext.SaveChangesAsync();

                return new OrderDto
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    CreatedAt = order.CreatedAt,
                    TotalAmount = order.TotalAmount,
                    Status = OrderStatus.Processing,
                    Items = order.Items.Select(item => new OrderItemDto
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    }).ToList() ?? new List<OrderItemDto>()
                };
            }catch(Exception ex)
            {                
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            return await _dbContext.Order
                .Include(o => o.Items)
                .Select(order => new OrderDto
                    {
                        Id = order.Id,
                        OrderNumber = order.OrderNumber,
                        CreatedAt = order.CreatedAt,
                        TotalAmount = order.TotalAmount,
                        Status = order.Status,
                        Items = order.Items.Select(item => new OrderItemDto
                        {
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        }).ToList()
                    }).ToListAsync();
        }
    }
}
