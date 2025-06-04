using OrderProcessing.Domain.Entity;
using System.Text.Json.Serialization;

namespace OrderProcessing.Application.DTO
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }
}
