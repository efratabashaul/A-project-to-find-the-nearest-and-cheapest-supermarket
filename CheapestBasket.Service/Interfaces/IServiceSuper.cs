using CheapestBasket.Repository;
using CheapestBasket.Repository.Entities;
using Common.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheapestBasket.Service.Interfaces
{
    public interface IServiceSuper<T>
    {
        public Task<T[]> BasketCloseSupers(ProductDto[] items);
        public Task<T[]> GetCloseSupers(string address, double distance, SuperDto[] supers);

    }
}
