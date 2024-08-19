using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheapestBasket.Repository.Entities
{
    public class Super
    {
        public string point_name { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public Product[] products { get; set; }
        public double Total { get; set; }
    }
}
