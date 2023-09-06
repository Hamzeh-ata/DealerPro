using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using test.Models;

namespace DealerPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeSearchController : ControllerBase
    {

        private readonly firebaseOptions _firebaseOptions;
        private FirebaseClient _firebaseClient;
        private readonly IMemoryCache _cache;
        private readonly List<Components> _components = new List<Components>();
        public HomeSearchController(IOptions<firebaseOptions> firebaseOptions, IMemoryCache cache)
        {
            _firebaseOptions = firebaseOptions.Value;
            _firebaseClient = new FirebaseClient(_firebaseOptions.DatabaseUrl);
            _cache = cache;
        }
        [HttpGet("{category}/{name}")]
        public async Task<ActionResult<List<Components>>> GetProductsByCategoryAndName(string category, string name)
        {
            var cacheKey = $"{category}_{name}";

            if (_cache.TryGetValue(cacheKey, out List<Components> components))
            {
                return components;
            }

            var productsQuery = await _firebaseClient.Child("Products").Child(category).OnceAsync<Dictionary<string, object>>();
            components = new List<Components>();

            foreach (var productSnapshot in productsQuery)
            {
                var productName = productSnapshot.Object["Name"] as string;
                if (!string.IsNullOrEmpty(productName) && productName.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    var component = new Components
                    {
                        ProductId = productSnapshot.Key,
                        Name = productName,
                        Price = (string)productSnapshot.Object["Price"],
                        OldPrice = productSnapshot.Object.ContainsKey("OldPrice") ? (string?)productSnapshot.Object["OldPrice"] : null,
                        Category = category,
                        Image = productSnapshot.Object["Image"] as string,
                        StoreName = productSnapshot.Object["StoreName"] as string,
                    };

                    components.Add(component);
                }
            }

            if (components.Count == 0)
            {
                return NotFound();
            }

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            _cache.Set(cacheKey, components, cacheOptions);

            return components;
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<List<Components>>> GetProductsByName(string name)
        {
            if (_cache.TryGetValue(name, out List<Components> components))
            {
                return components;
            }
            var categoriesQuery = await _firebaseClient.Child("Products").OnceAsync<Dictionary<string, object>>();
            var tasks = new List<Task>();
            components = new List<Components>();
            foreach (var categorySnapshot in categoriesQuery)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var productsQuery = await _firebaseClient.Child("Products").Child(categorySnapshot.Key).OnceAsync<Dictionary<string, object>>();
                    foreach (var productSnapshot in productsQuery)
                    {
                        var productName = productSnapshot.Object["Name"] as string;
                        if (!string.IsNullOrEmpty(productName) && productName.Contains(name, StringComparison.OrdinalIgnoreCase))
                        {
                            var component = new Components
                            {
                                ProductId = productSnapshot.Key,
                                Name = productName,
                                Price = (string)productSnapshot.Object["Price"],
                                OldPrice = productSnapshot.Object.ContainsKey("OldPrice") ? (string?)productSnapshot.Object["OldPrice"] : null,
                                Category = categorySnapshot.Key,
                                Image = productSnapshot.Object["Image"] as string,
                                StoreName = productSnapshot.Object["StoreName"] as string,
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
            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            _cache.Set(name, components, cacheOptions);

            return components;
        }
    }
}
