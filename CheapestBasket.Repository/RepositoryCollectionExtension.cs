using CheapestBasket.Repository.Entities;
using CheapestBasket.Repository.Interfaces;
using CheapestBasket.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheapestBasket.Repository
{
    public static class RepositoryCollectionExtension
    {
        //הגדרת התלויות
        public static IServiceCollection myAddRepositories(this IServiceCollection service)
        {
            service.AddTransient<SuperRepository>();
            service.AddScoped<IRepository<Product>, ProductRepository>();
            service.AddScoped<IRepositorySuper<Super>, SuperRepository>();
            return service;
        }
    }
}
