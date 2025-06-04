﻿using System.ComponentModel.DataAnnotations;

namespace OrderProcessing.Domain.Entity
{

    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal Total => Quantity * UnitPrice;
        public int OrderId { get; set; }
    }
}
