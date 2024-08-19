using CheapestBasket.Repository.Entities;
using CheapestBasket.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheapestBasket.Repository.Repositories
{
    public class SuperRepository:IRepositorySuper<Super>
    {
        static HttpClient client = new HttpClient();
        public SuperRepository()
        {
        }

        public async Task<HttpResponseMessage> getAll(Product product)
        {
            return await client.GetAsync("https://data.gov.il/api/3/action/datastore_search?resource_id=ef2bc38d-321a-4162-a4d2-ce806cf3f298&q={%22product%22:%22%D7" + product.product + "%22,%22importer%22:%22%D7" + product.importer + "%22,%22weight%22:%22%D7" + product.weight + "%22}");
        }

    }
}
