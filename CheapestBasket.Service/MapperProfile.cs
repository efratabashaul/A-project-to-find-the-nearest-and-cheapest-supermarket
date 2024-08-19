using AutoMapper;
using CheapestBasket.Repository.Entities;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheapestBasket.Service
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Super, SuperDto>().ReverseMap();
        }
    }
}
