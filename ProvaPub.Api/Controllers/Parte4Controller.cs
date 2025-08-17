using Microsoft.AspNetCore.Mvc;
using ProvaPub.Domain.Contracts;

namespace ProvaPub.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class Parte4Controller : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public Parte4Controller(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("CanPurchase")]
        public async Task<IActionResult> CanPurchase(int customerId, decimal purchaseValue)
        {
            return Ok(await _customerService.CanPurchase(customerId, purchaseValue));
        }
    }
}
