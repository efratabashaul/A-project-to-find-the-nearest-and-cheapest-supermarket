using AutoMapper;
using CheapestBasket.Repository.Entities;
using CheapestBasket.Repository.Interfaces;
using CheapestBasket.Repository.Repositories;
using CheapestBasket.Service.Interfaces;
using Common.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CheapestBasket.Service.Services
{
    public class SuperService : IServiceSuper<SuperDto>
    {
        private readonly IRepositorySuper<Super> repository;
        private readonly IMapper mapper;
        private readonly char[] CHARS_TO_TRIM = new char[] { 'k', 'm', 'm', 'i', 'n', 's' };
        HttpClient client = new HttpClient();
        public double DistanceInTime { get; set; }
        public SuperService(IRepositorySuper<Super> repository, IMapper _mapper)
        {
            this.repository = repository;
            mapper = _mapper;
        }

        private string TrimDistance(string distance)
        {
            string trimDistance = distance.Trim(CHARS_TO_TRIM);
            trimDistance.Trim();
            return trimDistance;
        }


        //פונקציה לחישוב מרחק בין 2 נקודות המיוצגות ב גוגלמפס
        public async Task<double> DistanceBetweenTwoPoints(string source, string destination)
        {
            if (!source.Equals(destination))
            {
                string url = "https://maps.googleapis.com/maps/api/distancematrix/xml?destinations=" + destination + "&origins=" + source + "&key=AIzaSyCZdcCyLVCdolqt4oR_KHpOwxoj8Wv4FaM";
                HttpResponseMessage httpResponse = await client.GetAsync(url);
                if (source.Equals("דסלר 8, בני בק"))
                    Console.WriteLine("httpResponse:  " + httpResponse);
                if (httpResponse.IsSuccessStatusCode)
                {
                    string responseXml = await httpResponse.Content.ReadAsStringAsync();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(responseXml);
                    if (source.Equals("דסלר 8, בני בק"))
                        Console.WriteLine("responseXml: " + " " + responseXml);
                    // Check if status is OK
                    XmlNode statusNode = xmlDoc.SelectSingleNode("//status");
                    if (statusNode != null && statusNode.InnerText == "OK")
                    {
                        XmlNode distanceNode = xmlDoc.SelectSingleNode("//distance/text");
                        if (distanceNode != null)
                        {
                            string distanceStr = distanceNode.InnerText;
                            return Convert.ToDouble(TrimDistance(distanceStr));
                        }
                        else
                        {
                            // Handle case when distanceNode is null-פונקציה הקוראת ע"י קוד של גוגל מפס לפונקציה טובה המנסה למצוא מרחק בכתובת בעייתית
                            return await GetDistanceFromSimilarAddress(source, destination);
                        }
                    }
                    else
                    {
                        // Handle case when status is not OK
                        Console.WriteLine("Google Maps API Error: " + statusNode?.InnerText);
                    }
                }
                else
                {
                    // Handle HTTP error
                    Console.WriteLine("HTTP Error: " + httpResponse.StatusCode);
                }
            }
            else
                return 0;
            return -1;
        }


        
        private async Task<double> GetDistanceFromSimilarAddress(string source, string destination)
        {
            try
            {
                string urlSimilar = $"https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input={Uri.EscapeDataString(source)}&inputtype=textquery&key=AIzaSyCZdcCyLVCdolqt4oR_KHpOwxoj8Wv4FaM";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseSimilar = await client.GetAsync(urlSimilar);

                    if (responseSimilar.IsSuccessStatusCode)
                    {
                        dynamic data = await responseSimilar.Content.ReadAsAsync<dynamic>();
                        if (data.status == "OK" && data.candidates != null && data.candidates.Count > 0)
                        {
                            string placeId = data.candidates[0].place_id;
                            return await GetDistanceFromPlaceId(placeId, destination, source);//פונקציה 
                        }
                        else
                        {
                            Console.WriteLine("No similar address found." + source);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {responseSimilar.StatusCode} - {responseSimilar.ReasonPhrase}" + source);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}" + source);
            }

            return -1;
        }


        //פונקציה יותר טובה המנסה למצוא מרחק בכתובת בעייתית
        private async Task<double> GetDistanceFromPlaceId(string placeId, string destination, string source)
        {
            try
            {
                // Construct the URL for the Google Maps Distance Matrix API
                string apiUrl = "https://maps.googleapis.com/maps/api/distancematrix/json";
                string apiKey = "AIzaSyCZdcCyLVCdolqt4oR_KHpOwxoj8Wv4FaM"; // Replace with your actual API key
                string url = $"{apiUrl}?origins=place_id:{placeId}&destinations={destination}&key={apiKey}";
                // Make the API request

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                        // Check if the status is OK
                        if (data.status == "OK")
                        {
                            if (data.rows[0].elements[0].status == "OK")
                            {
                                double distanceInMeters = (double)data.rows[0].elements[0].distance.value;
                                double distanceInKilometers = distanceInMeters / 1000; // Convert meters to kilometers
                                return distanceInKilometers;
                            }
                        }
                        else
                        {
                            // Handle the case when the status is not OK
                            Console.WriteLine($"Google Maps API error: {data.status}");
                        }
                    }
                    else
                    {
                        // Handle HTTP error
                        Console.WriteLine($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Error: {ex.Message}" + placeId + "   " + source);
            }

            // Return -1 if there's an error or the distance couldn't be retrieved
            return -1;
        }


        //פונקציה המחזירה את הסופרים שהתקבלו אך הנמצאים עד המרחק המבוקש
        public async Task<SuperDto[]> GetCloseSupers(string address, double distance, SuperDto[] supers)
        {
            List<SuperDto> closeSupers = new List<SuperDto>();
            double calculatedDistance;
            for (int i = 0; i < supers.Length; i++)
            {
                if (supers[i].address == null)//שליחה לפונקצית החישוב מרחק
                    calculatedDistance = await DistanceBetweenTwoPoints(supers[i].city, address);
                else
                {
                    if (supers[i].address.Contains(supers[i].city))//אם כתובת החנות מכילה גם את שם העיר-שולחת רק את הכתובת,שלא יחזור על עצמו
                        calculatedDistance = await DistanceBetweenTwoPoints(supers[i].address, address);
                    else//אחת-שולחת את הכתובת משורשרת עם העיר
                        calculatedDistance = await DistanceBetweenTwoPoints(supers[i].address + " " + supers[i].city, address);
                }

                // Check the calculated distance against the provided distance-אם קטן או שווה למרחק המבוקש מכניסה את החנות לרשימת החנויות המוחזרות
                if (calculatedDistance <= distance && calculatedDistance != -1)
                {
                    closeSupers.Add(supers[i]);
                }
            }
            SuperDto[] closeSupersArr = closeSupers.ToArray();
            double maxTotal = -1;
            int maxIndex = -1;

            //שמה את החנות הזולה ביתר בראש המערך-המקום הראשון
            for (int i = 0; i < closeSupersArr.Length; i++)
            {
                double sumSuper = 0;
                for (int j = 0; j < closeSupersArr[i].products.Length; j++)
                {
                    sumSuper += closeSupersArr[i].products[j].max_price;
                }
                closeSupersArr[i].Total = sumSuper;
                if (maxTotal < sumSuper)
                {
                    maxTotal = sumSuper;
                    maxIndex = i;
                }
            }
            if (maxIndex > 0)
            {
                SuperDto super = closeSupersArr[0];
                closeSupersArr[0] = closeSupersArr[maxIndex];
                closeSupersArr[maxIndex] = super;
            }
            return closeSupersArr;
        }


        //פונקציה המחזירה את הסופרים המתאימים ע"פ הסל הרצוי
        public async Task<SuperDto[]> BasketCloseSupers(ProductDto[] basket)
        {
            SuperDto[] closeSupersUnique=null;
            List<Super>[] supers = new List<Super>[basket.Length];
            for (int i = 0; i < supers.Length; i++)
            {
                    Product mapProduct = mapper.Map<Product>(basket[i]);
                    HttpResponseMessage response = await repository.getAll(mapProduct);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        JObject responseData = JObject.Parse(jsonString);
                        if (responseData["result"] != null && responseData["result"]["records"] is JArray records && records.Count > 0)
                        {
                            foreach (var record in records)
                            {
                                Super super = new Super
                                {
                                    point_name = record["point_name"].ToString(),
                                    city = record["city"].ToString(),
                                    address = record["address"].ToString()
                                };
                                Product product = new Product
                                {
                                    weight = record["weight"].ToString(),
                                    product = record["product"].ToString(),
                                    max_price = Convert.ToInt32(record["max_price"]),
                                    importer = record["importer"].ToString()
                                };
                                super.products = new Product[] { product };
                            //של החנויות אשר בהם המוצרים המבוקשים API-הוספה של כל החנויות החוזרות מה 
                            if (supers[i] == null)
                                    supers[i] = new List<Super> { super };
                                else
                                    supers[i].Add(super);
                            }
                        }
                    }
            }

            //הסרת הסופרים הכפולים
            List<Super> commonSupers = supers
                .Aggregate((current, next) => current
                .Where(s => next.Any(ns => ns.point_name == s.point_name && ns.city == s.city && ns.address == s.address))
                .ToList());

            if (commonSupers.Count < 3)
            {
                //הסף- במידה וחסר עד 10 אחוז מהסל-מוסיפה את החנויות אם כמות החנויות קטנה מ 3
               double threshold = supers.Length * 0.9;

                    // Retrieve the supers that exist in at least 90% of the lists
               List<Super> ninetyPercentSupers = supers
                   .SelectMany(s => s)
                   .GroupBy(s => s) // Group by Super object
                   .Where(g => g.Count() >= threshold) // Filter for objects in at least 90% of lists
                   .Select(g => g.Key) // Select the Super objects
                   .ToList();
                
               if (ninetyPercentSupers.Count > 0)
                  {
                    for (int i = 0; i < 3 - commonSupers.Count && ninetyPercentSupers[i] != null; i++)
                        {
                            commonSupers.Add(ninetyPercentSupers[i]);
                        }
                    }
                }


            //שרשור המוצרים בחנויות הכפולות לחנות אחת 
                foreach (Super commonSuper in commonSupers)
                {
                    foreach (List<Super> superList in supers)
                    {
                        Super matchingSuper = superList.FirstOrDefault(s => s.point_name == commonSuper.point_name && s.city == commonSuper.city && s.address == commonSuper.address);
                        if (matchingSuper != null)
                        {
                            var uniqueNewProducts = matchingSuper.products.Except(commonSuper.products);

                            // Concatenate unique new products to commonSuper's products
                            commonSuper.products = commonSuper.products.Concat(uniqueNewProducts).ToArray();
                        }
                    }
                }

                //הסרת החנויות הכפולות
                List<SuperDto> mapCommonSupers = mapper.Map<List<SuperDto>>(commonSupers);
                closeSupersUnique = mapCommonSupers
                    .GroupBy(s => new { s.point_name, s.address, s.city })
                    .Select(g => g.First())
                    .ToArray();
            return closeSupersUnique;
        }
    }
}
        
    
