﻿using CheapestBasket.Repository.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheapestBasket.Repository.Interfaces
{
    public interface IRepositorySuper<T>
    {
        public Task<HttpResponseMessage> getAll(Product item);
    }
}