using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.DTOs.Excel;
using SiaAdmin.Application.Interfaces.ConvertExcel;
using SiaAdmin.Application.Interfaces.Excel;
using SiaAdmin.Application.Interfaces.Firebase;
using SiaAdmin.Application.Interfaces.Sms;
using SiaAdmin.Infrastructure.Services;
using System.Net.Http.Headers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SiaAdmin.Application.Interfaces.SiaUser;

namespace SiaAdmin.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IExcelService, ExcelService>();
            services.AddSingleton<IConvertExcelFile, ConvertExcelFileService>();
            services.AddSingleton<ISmsService, SmsService>();
            services.AddSingleton<ISiaUserService, SiaUserService>();
            services.AddTransient<IAuthService, AuthService>(); 
            services.AddHttpClient<FirebaseService>(client =>
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddSingleton<FirebaseService>(sp=> {
                var httpClient = sp.GetRequiredService<HttpClient>();
                var firebaseApiKey = Configuration.FirebaseApiKey;
                return new FirebaseService(httpClient, firebaseApiKey);
            }); 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var projectId = Configuration.FirebaseAudience;
                    options.Audience = projectId;
                    options.Authority = Configuration.FirebaseAuthority;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = $"https://securetoken.google.com/{projectId}",
                        ValidateAudience = true,
                        ValidAudience = $"{projectId}",
                        ValidateLifetime = true
                    };
                });

          
        }
    }
}
