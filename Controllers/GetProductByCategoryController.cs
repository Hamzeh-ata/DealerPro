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
    public class GetProductByCategoryController : ControllerBase
    {

        private readonly firebaseOptions _firebaseOptions;
        private FirebaseClient _firebaseClient;
        private readonly IMemoryCache _cache;
        private readonly List<Components> _components = new List<Components>();
        public GetProductByCategoryController(IOptions<firebaseOptions> firebaseOptions, IMemoryCache cache)
        {
            _firebaseOptions = firebaseOptions.Value;
            _firebaseClient = new FirebaseClient(_firebaseOptions.DatabaseUrl);
            _cache = cache;
        }

        [HttpGet("{category}")]
        public async Task<ActionResult<List<Components>>> GetProductsByCategory(string category)
        {
            if (_cache.TryGetValue(category, out List<Components> components))
            {
                return components;
            }

            components = new List<Components>();

            var productsQuery = await _firebaseClient
                .Child("Products")
                .Child(category)
                .OnceAsync<Dictionary<string, object>>();

            foreach (var productSnapshot in productsQuery)
            {
                var component = new Components
                {
                    ProductId = productSnapshot.Key,
                    Name = productSnapshot.Object["Name"] as string,
                    Price = (string)productSnapshot.Object["Price"],
                    OldPrice = productSnapshot.Object.ContainsKey("OldPrice") ? (string?)productSnapshot.Object["OldPrice"] : null,
                    Category = category,
                    Image = productSnapshot.Object["Image"] as string,
                    StoreName = productSnapshot.Object["StoreName"] as string,
                };
                components.Add(component);
            }
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            _cache.Set(category, components, cacheOptions);
            return components;
        }
    }
}
