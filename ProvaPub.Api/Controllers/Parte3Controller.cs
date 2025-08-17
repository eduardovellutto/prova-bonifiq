using Microsoft.AspNetCore.Mvc;
using ProvaPub.Domain.Contracts;

namespace ProvaPub.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class Parte3Controller : ControllerBase
    {
        private readonly IOrderService _orderService;

        public Parte3Controller(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> PlaceOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            return Ok(await _orderService.PayOrder(paymentMethod, paymentValue, customerId));
        }
    }
}
