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
    public class GuideController : ControllerBase
    {
        private readonly firebaseOptions _firebaseOptions;
        private FirebaseClient _firebaseClient;
        private readonly IMemoryCache _cache;
        private readonly List<Components> _components = new List<Components>();
        public GuideController(IOptions<firebaseOptions> firebaseOptions, IMemoryCache cache)
        {
            _firebaseOptions = firebaseOptions.Value;
            _firebaseClient = new FirebaseClient(_firebaseOptions.DatabaseUrl);
            _cache = cache;
        }

        [HttpGet("{category}/{brand}/{price}")]
        public async Task<ActionResult<List<Components>>> GetProductsByCategoryAndBrand(string category, string brand, string price)
        {
            var cacheKey = $"{category}_{brand}_{price}";
            if (_cache.TryGetValue(cacheKey, out List<Components> components))
            {
                return components;
            }

            var productsQuery = await _firebaseClient.Child("Products").Child(category).OnceAsync<Dictionary<string, object>>();
            components = new List<Components>();

            foreach (var productSnapshot in productsQuery)
            {
                var productBrand = (productSnapshot.Object["Brand"] as string)?.ToLower();
                var productPrice = productSnapshot.Object["Price"] as string;
                decimal parsedProductPrice;
                if (productBrand == brand && !string.IsNullOrEmpty(productPrice) && decimal.TryParse(productPrice, out parsedProductPrice) && parsedProductPrice <= decimal.Parse(price))
                {
                    var component = new Components
                    {
                        ProductId = productSnapshot.Key,
                        Name = productSnapshot.Object["Name"] as string,
                        Price = productPrice,
                        OldPrice = productSnapshot.Object.ContainsKey("OldPrice") ? productSnapshot.Object["OldPrice"] as string : null,
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




    }
}
