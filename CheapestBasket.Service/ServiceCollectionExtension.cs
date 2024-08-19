using CheapestBasket.Repository;
using CheapestBasket.Repository.Entities;
using CheapestBasket.Service.Interfaces;
using CheapestBasket.Service.Services;
using Common.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheapestBasket.Service
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection myAddServices(this IServiceCollection service)
        {
            service.AddTransient<SuperService>();
            service.myAddRepositories();
            service.AddScoped<IService<ProductDto>, ProductService>();
            service.AddScoped<IServiceSuper<SuperDto>, SuperService>();

            return service;
        }
    }
}
