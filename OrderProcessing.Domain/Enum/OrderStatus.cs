namespace OrderProcessing.Domain.Enum
{
    public enum OrderStatus
    {
        Pending = 0,
        Processing = 1,
        Shipped = 2,
        Delivered = 3,
        Canceled = 4,
        Failed = 5
    }
}
