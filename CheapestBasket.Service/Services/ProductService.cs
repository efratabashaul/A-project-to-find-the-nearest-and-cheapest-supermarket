using AutoMapper;
using CheapestBasket.Repository.Entities;
using CheapestBasket.Repository.Interfaces;
using CheapestBasket.Service.Interfaces;
using Common.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CheapestBasket.Service.Services
{
    public class ProductService : IService<ProductDto>
    {
        private readonly IRepository<Product> repository;
        private readonly IMapper mapper;

        public ProductService(IRepository<Product> repository, IMapper _mapper)
        {
            this.repository = repository;
            this.mapper = _mapper;
        }

        public async Task<ProductDto[]> getAll()
        {
            HttpResponseMessage response= await repository.getAll();
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                JObject responseData = JObject.Parse(jsonString);

                if (responseData["result"] != null)
                {
                    JArray records = (JArray)responseData["result"]["records"];
                    Product[] products = records.ToObject<Product[]>();
                    ProductDto[] dtoProducts = mapper.Map<ProductDto[]>(products);
                    ProductDto[] uniqueProducts = dtoProducts
                        .GroupBy(p => new { p.importer, p.product, p.weight })
                        .Select(group => group.First())
                        .ToArray();
                    return uniqueProducts;
                }
                else
                {
                    Console.WriteLine("No records found in the response.");
                }
            }
            else
            {
                Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
            }
            return null;
        }
    }
}
