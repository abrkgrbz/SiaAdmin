using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json;
using SiaAdmin.Application.Interfaces.Firebase;
using SiaAdmin.Application.Interfaces.Firebase.Models;
using ErrorInfo = FirebaseAdmin.Auth.ErrorInfo;

namespace SiaAdmin.Infrastructure.Services
{
    public class FirebaseService:IFirebaseService  
    {
        private readonly HttpClient _httpClient;
        private readonly string _firebaseApiKey;

        public FirebaseService(HttpClient httpClient, string firebaseApiKey)
        {
            _httpClient = httpClient;
            _firebaseApiKey = firebaseApiKey;
        }

        public async Task<string> CreateCustomTokenAsync(string userId)
        {
            try
            {

                var additionalClaims = new Dictionary<string, object>()
            {
                { "userVerify", true },
            };

                string customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(userId, additionalClaims);

                return customToken;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }

        public async Task<FirebaseAuthResponse> SignInWithCustomTokenAsync(string token, bool returnSecureToken)
        {
            var requestBody = new
            {
                token = token,
                returnSecureToken = returnSecureToken
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken?key={_firebaseApiKey}", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(responseContent);
            }

            return JsonConvert.DeserializeObject<FirebaseAuthResponse>(responseContent);
        }

        public async Task<FirebaseToken> VerifyIdTokenAsync(string idToken)
        {
            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(idToken);

                string uid = decodedToken.Uid;

                return decodedToken;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserImportResult> ImportUsersAsync(List<ImportUserRecordArgs> users)
        {
            try
            {
                UserImportResult result = await FirebaseAuth.DefaultInstance.ImportUsersAsync(users);
                foreach (ErrorInfo indexedError in result.Errors)
                {
                    Console.WriteLine($"Failed to import user: {indexedError.Reason}");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
 
        public async Task<FirebaseToken> DecodeIdTokenAsync(string idToken)
        {
            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(idToken);

                return decodedToken;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserRecord> GetUserWithIdAsync(string uid)
        {
            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
                return userRecord;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RefreshToken> GetTokenWithRefreshToken(string refreshToken)
        {
            var requestBody = new
            {
                refresh_token = refreshToken,
                grant_type = "refresh_token"
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"https://securetoken.googleapis.com/v1/token?key={_firebaseApiKey}", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(responseContent);
            }
            
            return JsonConvert.DeserializeObject<RefreshToken>(responseContent);
        }

        public async Task<dynamic> SendMessagesToMultipleDevice(List<string> tokenList, string title, string body, Dictionary<string, string> data)
        {
            var message = new MulticastMessage()
            {
                Tokens = tokenList,
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                },
                Data = data
            };


            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
            if (response.FailureCount > 0)
            {
                var failedTokens = new List<string>();
                for (var i = 0; i < response.Responses.Count; i++)
                {
                    if (!response.Responses[i].IsSuccess)
                    {
                        // The order of responses corresponds to the order of the registration tokens.
                        failedTokens.Add(tokenList[i]);
                    }
                }

                Console.WriteLine($"List of tokens that caused failures: {failedTokens}");
            }

            return true;
        }
    }
}
