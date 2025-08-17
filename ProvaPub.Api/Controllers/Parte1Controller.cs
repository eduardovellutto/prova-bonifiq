using Microsoft.AspNetCore.Mvc;
using ProvaPub.Domain.Contracts;

namespace ProvaPub.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Parte1Controller : ControllerBase
    {
        private readonly IRandonService _randomService;

        public Parte1Controller(IRandonService randomService)
        {
            _randomService = randomService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _randomService.GetRandom());
        }
    }
}
