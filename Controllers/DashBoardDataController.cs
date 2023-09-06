using DealerPro.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FireSharp.Config;
using FireSharp.Interfaces;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace DealerPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardDataController : ControllerBase
    {
        IFirebaseClient client;
        private Dictionary<string, dynamic> _data;
        private readonly string endPoint = "/customStores";
        private readonly IConfiguration configuration;
        IFirebaseConfig config;
        FirebaseApp firebaseApp = null;
        string appName = "getStoreName";

        public DashBoardDataController(IConfiguration configuration)
        {
            this.configuration = configuration;
            var firebaseConfig = configuration.GetSection("Firebase");

            config = new FirebaseConfig
            {
                BasePath = firebaseConfig["DatabaseUrl"],
                AuthSecret = firebaseConfig["ApiKey"]
            };
        }
        [HttpPost("userid")]
        public IActionResult GetUserId([FromBody] TokenData tokenData)
        {
            try
            {
                // Validate the token and check if it has expired
                var decodedToken = FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(tokenData.Token).Result;
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(decodedToken.ExpirationTimeSeconds);
                var currentTime = DateTimeOffset.UtcNow;

                if (currentTime > expirationTime)
                {
                    // Token has expired
                    return Unauthorized();
                }

                var userId = decodedToken.Uid;
                return Ok(userId);
            }
            catch (FirebaseAuthException)
            {
                // Token validation failed
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid token: " + ex.Message);
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateNewStore([FromBody] storeData storedata)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                var response = await client.GetAsync(endPoint);
                _data = response.ResultAs<Dictionary<string, dynamic>>();

                if (_data != null)
                {
                    if (await CheckForStoreNameDuplicates(storedata.storeName) && await CheckForUIDDuplicates(storedata.UID))
                    {
                        await client.PushAsync(endPoint, new { StoreName = storedata.storeName, UserId = storedata.UID });
                        return Ok("New store created");
                    }
                    else if (!await CheckForStoreNameDuplicates(storedata.storeName))
                    {
                        return StatusCode((int)HttpStatusCode.Conflict, "Store name already exists");
                    }
                    else if (!await CheckForUIDDuplicates(storedata.UID))
                    {
                        return StatusCode((int)HttpStatusCode.Forbidden, "You can only have one store");
                    }
                }
                else if (_data == null)
                {
                    await client.PushAsync(endPoint, new { StoreName = storedata.storeName, UserId = storedata.UID });
                    return Ok("New store created");
                }

                return Ok("Store created");
            }
            catch (Exception e)
            {
                // Log the exception
                return BadRequest("An error occurred while processing the request");
            }
        }
        [HttpPut("changeName")]
        public async Task<IActionResult> ChangeStoreName(string UID, string existStoreName, string newStoreName)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                var response = await client.GetAsync(endPoint);
                _data = response.ResultAs<Dictionary<string, dynamic>>();

                if (!await CheckForStoreNameDuplicates(existStoreName) && !await CheckForUIDDuplicates(UID))
                {
                    var existingItem = _data.FirstOrDefault(x => x.Value.StoreName == existStoreName && x.Value.UserId == UID);
                    var existingKey = existingItem.Key;
                    await client.UpdateAsync(endPoint + "/" + existingKey, new { StoreName = newStoreName, UserId = UID });

                    return Ok("Store name changed");
                }
                else
                {
                    return BadRequest("Error: Invalid store name or UID");
                }
            }
            catch (Exception e)
            {
                // Log the exception
                return BadRequest("An error occurred while processing the request");
            }
        }
        [HttpDelete("deleteStore")]
        public async Task<IActionResult> DeleteStore(string UID, string storeName)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                var response = await client.GetAsync(endPoint);
                _data = response.ResultAs<Dictionary<string, dynamic>>();

                if (!await CheckForStoreNameDuplicates(storeName) && !await CheckForUIDDuplicates(UID))
                {
                    var existingItem = _data.FirstOrDefault(x => x.Value.StoreName == storeName && x.Value.UserId == UID);
                    if (existingItem.Value != null)
                    {
                        var existingKey = existingItem.Key;
                        await client.DeleteAsync(endPoint + "/" + existingKey);
                        return Ok("Store removed");
                    }
                    else
                    {
                        return NotFound("Store not found");
                    }
                }
                else
                {
                    return BadRequest("Error: Invalid store name or UID");
                }
            }
            catch (Exception e)
            {
                // Log the exception
                return BadRequest("An error occurred while processing the request");
            }
        }
        [HttpGet("getStoreName")]
        public async Task<IActionResult> GetStoreName(string Token)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                var response = await client.GetAsync(endPoint);
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
                var decodedToken = auth.VerifyIdTokenAsync(Token).Result;
                var userId = decodedToken.Uid;
                if (_data != null)
                {
                    foreach (var item in _data)
                    {
                        var itemId = item.Key;
                        var storeData = item.Value["StoreName"];
                        var storeUID = item.Value["UserId"];

                        if (storeUID == userId)
                        {
                            string storeName = storeData;
                            return Ok(storeName);
                        }
                    }
                }
                return NotFound();
            }
            catch (Exception e)
            {
                // Log the exception
                return BadRequest("An error occurred while processing the request");
            }
        }

        [HttpPost("CheckForStoreDuplicate")]
        public async Task<IActionResult> CheckForStoreDuplicate([FromBody] string storeName)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                var response = await client.GetAsync(endPoint);
                _data = response.ResultAs<Dictionary<string, dynamic>>();

                if (_data != null)
                {
                    if (await CheckForStoreNameDuplicates(storeName))
                    {
                        return Ok();
                    }
                    else if (!await CheckForStoreNameDuplicates(storeName))
                    {
                        return BadRequest("Store name already exists");
                    }
                }
                else if (_data == null)
                {

                    return Ok();
                }
                return BadRequest("Their is an Duplicate for this store");

            }
            catch (Exception e)
            {
                return BadRequest("An error occurred while processing the request" + e.ToString);
            }
        }


        private async Task<bool> CheckForStoreNameDuplicates(string storeName)
        {
            foreach (var item in _data)
            {
                var existingStoreName = item.Value.StoreName;
                if (existingStoreName == storeName)
                {
                    return false;
                }
            }
            return true;
        }
        private async Task<bool> CheckForUIDDuplicates(string uID)
        {
            foreach (var item in _data)
            {
                var existingUID = item.Value.UserId;
                if (existingUID == uID)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
