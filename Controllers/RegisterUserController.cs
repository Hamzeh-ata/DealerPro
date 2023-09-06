using DealerPro.Models;
using Firebase.Database;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.Models;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net;
using Google.Apis.Auth;

namespace DealerPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterUserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private static Dictionary<string, FirebaseApp> initializedApps = new Dictionary<string, FirebaseApp>();
        FirebaseApp firebaseApp = null;
        string appName = "Sign up";
        public RegisterUserController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> registerUser([FromBody] RegistrationData registrationData)
        {
            try
            {
                // Load Firebase configuration from app settings
                // Load Firebase configuration from app settings
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

                // Initialize FirebaseApp with Firebase configuration from app settings
                if (await IsEmailExists(registrationData.Email))
                {
                    return Conflict("Email already exists");
                }
                if (!await IsValidEmail(registrationData.Email))
                {
                    return BadRequest("Invalid email address");
                }
                if (!await passwordCount(registrationData.Password))
                {
                    return BadRequest("Password must be at least 6 characters long");
                }
                if (!await HasNumberAndLetter(registrationData.Password))
                {
                    return BadRequest("Password must contain both numbers and letters");
                }
                // Create a new user with email and password
                var auth = FirebaseAuth.GetAuth(firebaseApp);
                UserRecord user;
                user = await auth.CreateUserAsync(new UserRecordArgs
                {
                    Email = registrationData.Email,
                    Password = registrationData.Password,
                });
                // Check if email already exists
                // User registration successful
                // Generate an authentication token for the registered user
                // Generate an authentication token for the registered user
                var customToken = await auth.CreateCustomTokenAsync(user.Uid);
                // Return the authentication token and user ID to the client
                return Ok(new { Token = customToken, Uid = user.Uid });
            }

            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
    }



        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUserWithGoogle([FromBody] GoogleRegistrationData registrationData)
        {
            try
            {
                // Load Firebase configuration from app settings
                var firebaseConfig = configuration.GetSection("Firebase");

                // Verify and decode the Google ID token
                var payload = await GoogleJsonWebSignature.ValidateAsync(registrationData.GoogleIdToken, new GoogleJsonWebSignature.ValidationSettings());

                // Retrieve user information from the verified token
                var email = payload.Email;
                var displayName = payload.Name;
                var googleUserId = payload.Subject;

                // Check if the user already exists with the given email
                if (await IsEmailExists(email))
                {
                    return Conflict("Email already exists");
                }

                // Create a new user with the Google user ID as the UID
                var auth = FirebaseAuth.GetAuth(firebaseApp);
                var user = await auth.CreateUserAsync(new UserRecordArgs
                {
                    Uid = googleUserId,
                    DisplayName = displayName,
                    Email = email
                });

                // Generate an authentication token for the registered user
                var customToken = await auth.CreateCustomTokenAsync(user.Uid);

                // Return the authentication token and user ID to the client
                return Ok(new { Token = customToken, Uid = user.Uid });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }






private async Task<bool> IsValidEmail(string email)
        {
            // Use a regular expression to validate the email format
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }
private async Task<bool> IsEmailExists(string email)
        {
            try
            {
                var auth = FirebaseAuth.GetAuth(firebaseApp);
                var user = await auth.GetUserByEmailAsync(email);
                return user != null;
            }
            catch (FirebaseAuthException)
            {
                return false; // User does not exist
            }

        }
 private async Task<bool> passwordCount(string password)
        {
            if (password.Count() < 6)
            {
                return false;
            }
            return true;
        }
 private async Task<bool> HasNumberAndLetter(string password)
        {
           // Regular expression to match at least one digit and one letter
    Regex regex = new Regex(@"^(?=.*[0-9])(?=.*[a-zA-Z]).*$");
            // Test the password against the regular expression
            return regex.IsMatch(password);
        }

    }
   
    }


