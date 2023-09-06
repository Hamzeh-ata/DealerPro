using Firebase.Database;
using Firebase.Database.Query;
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
    public class ProductDetailsController : ControllerBase
    {
        private readonly firebaseOptions _firebaseOptions;
        private FirebaseClient _firebaseClient;
        private readonly IMemoryCache _cache;
        private readonly List<Components> _components = new List<Components>();
        public ProductDetailsController(IOptions<firebaseOptions> firebaseOptions, IMemoryCache cache)
        {
            _firebaseOptions = firebaseOptions.Value;
            _firebaseClient = new FirebaseClient(_firebaseOptions.DatabaseUrl);
            _cache = cache;
        }
        [HttpGet("{category}/{id}")]
        public async Task<ActionResult<List<Components>>> GetProductDeatils(string category, string id)
        {
            if (_cache.TryGetValue($"{category}_{id}", out List<Components> components))
            {
                return components;
            }

            components = new List<Components>();

            var productQuery = await _firebaseClient
                .Child("Products")
                .Child(category)
                .Child(id)
                .OnceSingleAsync<Dictionary<string, object>>();
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

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            _cache.Set($"{category}_{id}", components, cacheOptions);

            return components;
        }
    }
}
