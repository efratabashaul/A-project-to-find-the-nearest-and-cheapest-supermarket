using CheapestBasket.Repository.Entities;
using CheapestBasket.Service.Interfaces;
using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheapestBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IService<ProductDto> service;

        public ProductController(IService<ProductDto> service)
        {
            this.service = service;
        }
        [HttpGet]
        public async Task<ProductDto[]> GetUniqueProducts()
        {
            return await service.getAll();
        }
        
    }
}
