using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.Interface;
using OrderProcessing.Domain.Entity;

namespace OrderProcessing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            try
            {
                var productResult = await _orderService.CreateOrderAsync(order);
                return Ok(new { productResult });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
