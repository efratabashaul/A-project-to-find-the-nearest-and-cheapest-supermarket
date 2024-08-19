using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class ProductDto
    {
        public string importer { get; set; }
        public string product { get; set; }
        public string weight { get; set; }
        public double max_price { get; set; }
    }
}
