using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using FireSharp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using test.Models;

namespace DealerPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class dashBoardProductsController : ControllerBase
    {
        IFirebaseClient client;
        private Dictionary<string, dynamic> _data;
        private readonly string endPoint = "/Products";
        private readonly IConfiguration configuration;
        IFirebaseConfig config;
        FirebaseApp firebaseApp = null;
        string appName = "getProduct";
        private readonly IMemoryCache _cache;
        private FirebaseClient _firebaseClient;
        private readonly List<Components> _components = new List<Components>();
        private readonly firebaseOptions _firebaseOptions;
        public dashBoardProductsController(IOptions<firebaseOptions> firebaseOptions, IMemoryCache cache)
        {
            _firebaseOptions = firebaseOptions.Value;
            _firebaseClient = new FirebaseClient(_firebaseOptions.DatabaseUrl);
            _cache = cache;
        }
        [HttpGet("{storeName}")]
        public async Task<ActionResult<List<Components>>> getStoreProducts(string storeName)
        {
            var categoriesQuery = await _firebaseClient.Child("Products").OnceAsync<Dictionary<string, object>>();
            var tasks = new List<Task>();
            var components = new List<Components>();

            foreach (var categorySnapshot in categoriesQuery)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var productsQuery = await _firebaseClient.Child("Products").Child(categorySnapshot.Key).OnceAsync<Dictionary<string, object>>();
                    foreach (var productSnapshot in productsQuery)
                    {
                        var productStoreName = productSnapshot.Object["StoreName"] as string;
                        if (!string.IsNullOrEmpty(productStoreName) && productStoreName.Equals(storeName))
                        {
                            var component = new Components
                            {
                                ProductId = productSnapshot.Key,
                                Name = (string)productSnapshot.Object["Name"],
                                Price = (string)productSnapshot.Object["Price"],
                                OldPrice = productSnapshot.Object.ContainsKey("OldPrice") ? (string?)productSnapshot.Object["OldPrice"] : null,
                                Category = categorySnapshot.Key,
                                Image = productSnapshot.Object["Image"] as string,
                                StoreName = storeName,
                            };
                            lock (components)
                            {
                                components.Add(component);
                            }
                        }
                    }
                }));
            }
            await Task.WhenAll(tasks);

            if (components.Count == 0)
            {
                return NotFound();
            }

            return components;
        }

        [HttpGet("{category}/{id}")]
        public async Task<ActionResult<List<Components>>> GetProductDeatils(string category, string id)
        {
            var components = new List<Components>();
            var productQuery = await _firebaseClient
                .Child("Products")
                .Child(category)
                .Child(id)
                .OnceSingleAsync<Dictionary<string, object>>();
            // Split the description string into a list of strings using the newline character as the delimiter
            var descriptionJArray = (JArray)productQuery["description"];
            var descriptionList = descriptionJArray.ToObject<List<string>>();
            var component = new Components
            {
                ProductId = id,
                Name = productQuery["Name"] as string,
                Price = (string)productQuery["Price"],
                OldPrice = productQuery.ContainsKey("OldPrice") ? (string?)productQuery["OldPrice"] : "0",
                Category = category,
                Image = productQuery["Image"] as string,
                StoreName = productQuery["StoreName"] as string,
                Brand = productQuery["Brand"] as string,
                ProductUrl = productQuery["ProductUrl"] as string,
                Time = productQuery["Time"] as string,
                Date = productQuery["Date"] as string,
                description = descriptionList,
            };

            components.Add(component);
            return components;
        }

      



    }
}
