using Microsoft.AspNetCore.Mvc;
using ProvaPub.Domain.Contracts;

namespace ProvaPub.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class Parte2Controller : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public Parte2Controller(IProductService productService, ICustomerService customerService)
        {
            _productService = productService;
            _customerService = customerService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> ListProducts(int? page)
        {
            return Ok(await _productService.GetListAsync(page));
        }

        [HttpGet("customers")]
        public async Task<IActionResult> ListCustomers(int? page)
        {
            return Ok(await _customerService.GetListAsync(page));
        }
    }
}
