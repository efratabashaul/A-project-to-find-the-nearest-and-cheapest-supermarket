using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheapestBasket.Repository.Entities
{
    public class Product
    {
        public string importer { get; set; }
        public string product { get; set; }
        public string weight { get; set; }
        public double max_price { get; set; }
    }
}
