using Microsoft.EntityFrameworkCore;
using OrderProcessing.Domain.Entity;
using static OrderProcessing.Domain.Entity.Order;

namespace OrderProcessing.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
