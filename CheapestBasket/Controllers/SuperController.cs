using CheapestBasket.Service.Interfaces;
using CheapestBasket.Service.Services;
using Common.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheapestBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperController : ControllerBase
    {
        private readonly SuperService service;
        public SuperController(SuperService service)
        {
            this.service = service;
        }
        [HttpPost("{address}/{distance}")]
        public async Task<SuperDto[]> GetCloseSupers([FromBody] SuperDto[] supers, string address, double distance)
        {
            return await service.GetCloseSupers(address, distance, supers);
        }
        [HttpPost]
        public async Task<SuperDto[]> BasketCloseSupers([FromBody] ProductDto[]basket)
        {
            return await service.BasketCloseSupers(basket);
        }
    }
}
