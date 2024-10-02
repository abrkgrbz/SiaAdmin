using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using SiaAdmin.Application.Interfaces.Firebase;

namespace SiaAdmin.Infrastructure.Services
{
    public class AuthService:IAuthService
    {
        private readonly HttpClient? _httpClient;

        public AuthService(HttpClient? httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Login(string phoneNumber)
        {
            var request = new
            {
                phoneNumber,
                returnSecureToken = true
            };
            var response = await _httpClient.PostAsJsonAsync("", request);
            var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();
            return authToken.IdToken;
        }

        public class AuthToken
        {
            [JsonPropertyName("idToken")] public string IdToken { get; set; }
            [JsonPropertyName("email")] public string Email { get; set; }
            [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; }
            [JsonPropertyName("expiresIn")] public string ExpiresIn { get; set; }
            [JsonPropertyName("localId")] public string LocalId { get; set; }
        }
  
    }
}
