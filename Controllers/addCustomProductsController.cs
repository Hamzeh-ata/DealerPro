using DealerPro.Models;
using Firebase.Storage;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FireSharp.Config;
using FireSharp.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DealerPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class addCustomProductsController : ControllerBase
    {
        IFirebaseClient client;
        private static bool firebaseInitialized = false;
        private Dictionary<string, dynamic> _data;
        private readonly string endPoint = "/Products";
        private readonly IConfiguration configuration;
        IFirebaseConfig config;
        FirebaseApp firebaseApp = null;
        string appName = "addProduct";
        public addCustomProductsController(IConfiguration configuration)
        {
            this.configuration = configuration;
            var firebaseConfig = configuration.GetSection("Firebase");

            config = new FirebaseConfig
            {
                BasePath = firebaseConfig["DatabaseUrl"],
                AuthSecret = firebaseConfig["ApiKey"]
            };
        }
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromForm] customStoreProductData model)
        {
            try
            {
                var productEndPoint = endPoint + "/" + model.Category;

                client = new FireSharp.FirebaseClient(config);
                var response = await client.GetAsync(productEndPoint);
                _data = response.ResultAs<Dictionary<string, dynamic>>();

                var firebaseConfig = configuration.GetSection("Firebase");
                var requestUri = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyALF_NBlHu5YgYPavUklfiemW69LBImXjw";
                try
                {
                    firebaseApp = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(new
                        {
                            type = firebaseConfig["type"],
                            project_id = firebaseConfig["project_id"],
                            private_key_id = firebaseConfig["private_key_id"],
                            private_key = firebaseConfig["private_key"],
                            client_email = firebaseConfig["client_email"],
                            client_id = firebaseConfig["client_id"],
                            auth_uri = firebaseConfig["auth_uri"],
                            token_uri = firebaseConfig["token_uri"],
                            auth_provider_x509_cert_url = firebaseConfig["auth_provider_x509_cert_url"],
                            client_x509_cert_url = firebaseConfig["client_x509_cert_url"],
                            universe_domain = firebaseConfig["universe_domain"]
                        }))
                    }, appName);
                }
                catch (ArgumentException ex)
                {
                    // Firebase app with the same name already exists
                    firebaseApp = FirebaseApp.GetInstance(appName);
                }
                var auth = FirebaseAuth.GetAuth(firebaseApp);
                var decodedToken = auth.VerifyIdTokenAsync(model.UID).Result;
                var userId = decodedToken.Uid;
                model.UID = userId;
                // Check if a file was actually uploaded
                if (model.Img != null && model.Img.Length > 0)
                {
                    // Generate a unique filename for the image
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Img.FileName);
                    // Create a Firebase Storage reference with the desired filename
                    var storageReference = new FirebaseStorage("test-811c3.appspot.com")
                        .Child("images")
                        .Child(fileName);

                    // Upload the file to Firebase Storage
                    using (var stream = model.Img.OpenReadStream())
                    {
                        await storageReference.PutAsync(stream);
                    }

                    // Get the download URL for the file
                    var imageUrl = await storageReference.GetDownloadUrlAsync();

                    // Wait until the image is uploaded and the URL is generated
                    model.Image = imageUrl;

                    // Save the complete model to the Firebase Realtime Database
                    if (_data == null)
                    {
                        await client.PushAsync(productEndPoint, model);
                    }
                    else
                    {
                        if (await checkForProductDublicate(model.StoreName, model.Name))
                        {
                            await client.PushAsync(productEndPoint, model);
                            return Ok("productAdded");
                        }
                        else if (!await checkForProductDublicate(model.StoreName, model.Name))
                        {
                            return BadRequest("Product already exists");
                        }

                    }

                }
                model.Time = DateTime.Now.ToString("HH:mm");
                model.Date = DateTime.Now.ToString("yyyy-MM-dd");
                return Ok("productAdded");
            }
            catch (Exception ex)
            {
                // Handle any errors that occurred during the process
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("updateProduct/{category}/{productId}")]
        public async Task<IActionResult> UpdateProduct(string category, string productId, [FromForm] updateProducts model)
        {
            try
            {
                var productEndPoint = endPoint + "/" + category;

                client = new FireSharp.FirebaseClient(config);
                var response = await client.GetAsync(productEndPoint);
                var data = response.ResultAs<Dictionary<string, dynamic>>();

                bool productExists = data.ContainsKey(productId);

                if (productExists)
                {
                    // Update the data of the product
                    data[productId].StoreName = model.StoreName;
                    data[productId].Name = model.Name;
                    data[productId].Price = model.Price;
                    data[productId].Brand = model.Brand;
                    data[productId].ProductUrl = model.ProductUrl;
                    data[productId].OldPrice = model.OldPrice != null ? model.OldPrice : "0";
                    data[productId].description = JArray.FromObject(model.description);
                    data[productId].Time = DateTime.Now.ToString("HH:mm");
                    data[productId].Date = DateTime.Now.ToString("yyyy-MM-dd");
                    if (model.Img != null && model.Img.Length > 0)
                    {
                        // Generate a unique filename for the image
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Img.FileName);
                        // Create a Firebase Storage reference with the desired filename
                        var storageReference = new FirebaseStorage("test-811c3.appspot.com")
                            .Child("images")
                            .Child(fileName);

                        // Upload the file to Firebase Storage
                        using (var stream = model.Img.OpenReadStream())
                        {
                            await storageReference.PutAsync(stream);
                        }

                        // Get the download URL for the file
                        var imageUrl = await storageReference.GetDownloadUrlAsync();

                        // Update the image URL in the product data
                        data[productId].Image = imageUrl;
                    }
                    // Update the product in Firebase

                    if (await CheckForProductDuplicate(category, model.StoreName, model.Name, productId))
                    {
                        await client.SetAsync(productEndPoint, data);
                        return Ok("productUpdated");
                    }
                    else if(!await CheckForProductDuplicate(category, model.StoreName, model.Name, productId))
                    {
                        return Ok("Product Name already exists in same store");
                    }
                    return Ok("productUpdated");
                }
                else
                {
                    return NotFound("Product not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            
            }
            }

        [HttpDelete("deleteProduct/{category}/{productId}")]
        public async Task<IActionResult> DeleteProduct(string category,string productId)
        {
            try
            {
                var productEndPoint = endPoint + "/" + category + "/" + productId;
                client = new FireSharp.FirebaseClient(config);
                var response = await client.GetAsync(productEndPoint);
                var data = response.ResultAs<Dictionary<string, dynamic>>();
                await client.DeleteAsync(productEndPoint);
                return Ok("Product deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        private async Task<bool> checkForProductDublicate(string storeName, string productName)
        {
            foreach (var item in _data)
            {
                var existingName = item.Value.Name;
                var existingStoreName = item.Value.StoreName;
                if (existingName == productName && existingStoreName == storeName)
                {
                    return false;
                }
            }
            return true;
        }
     private async Task<bool> CheckForProductDuplicate(string category, string storeName, string productName, string id)
        {
            var productEndPoint = endPoint + "/" + category;
            var response = await client.GetAsync(productEndPoint);
            var data = response.ResultAs<Dictionary<string, dynamic>>();

            foreach (var entry in data)
            {
                var productId = entry.Key;
                var product = entry.Value;

                if (product.StoreName == storeName && product.Name == productName && productId!=id)
                {
                    Console.WriteLine(productId);
                    Console.WriteLine(id+"2");
                    // Found a duplicate product
                    return false;
                }
               
            }

            // No duplicate product found
            return true;
        }



    }




}





















