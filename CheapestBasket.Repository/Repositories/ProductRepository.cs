using CheapestBasket.Repository.Entities;
using CheapestBasket.Repository.Interfaces;
//using MockContext;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace CheapestBasket.Repository.Repositories
{
    public class ProductRepository:IRepository<Product>
    {
        //private readonly IContext _context;
        static HttpClient client = new HttpClient();
        public ProductRepository(/*IContext context*/)
        {
            //_context = context;
        }


        public async Task<HttpResponseMessage> getAll()
        {
            return await client.GetAsync("https://data.gov.il/api/3/action/datastore_search?resource_id=ef2bc38d-321a-4162-a4d2-ce806cf3f298");
        }

    }
}
