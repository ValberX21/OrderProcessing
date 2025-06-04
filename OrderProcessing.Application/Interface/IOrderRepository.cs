using OrderProcessing.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Application.Interface
{
    public interface IOrderRepository
    {
        Task<OrderDto> AddAsync(Order order);
    }
}
