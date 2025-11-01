using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.OrderService.Entities;
using Play.OrderService.Repo;

namespace Play.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderClass order;
        public OrderController(IOrderClass order)
        {
            this.order = order;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery]Guid userId)
        {
            await order.CreateOrder(userId);
            return Ok("order created successfully");
        }
        [HttpGet]
        public async Task<IActionResult> GetUserOrders([FromQuery] Guid userId)
        {
            var orders = await order.GetUserOrder(userId);
            return Ok(orders);
        }
       
    }
}
