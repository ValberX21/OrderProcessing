using OrderProcessing.Domain.Enum;

namespace OrderProcessing.Domain.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public List<OrderItem> Items { get; set; } = new();

    }
}
