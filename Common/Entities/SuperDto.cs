using CheapestBasket.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class SuperDto
    {
        public string point_name { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public ProductDto[] products { get; set; }
        public double Total { get; set; }
    }
}
