using System.Security.Claims;
using System.Text;
using System.Text.Json;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Identity;
using SiaAdmin.Application.Interfaces.Firebase.Models;

namespace SiaAdmin.API.Middlewares
{
    public class TokenDecodeMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenDecodeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null)
            {
                context.Items.Clear();
                string accessTokenWithoutBearerPrefix = authHeader.Substring("Bearer ".Length);
                LookupRequest newToken = new() { IdToken = accessTokenWithoutBearerPrefix };
                bool checkRevoked = true;
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(
                    newToken.IdToken, checkRevoked);
                string uid = decodedToken.Uid;
                context.Items.Add("userGuid",uid);
            }
            
            await _next(context);
        }
    }
}
