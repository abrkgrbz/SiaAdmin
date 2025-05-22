 
using Microsoft.Extensions.DependencyInjection; 
using SiaAdmin.Application.Interfaces.ConvertExcel;
using SiaAdmin.Application.Interfaces.Excel;
using SiaAdmin.Application.Interfaces.Firebase;
using SiaAdmin.Application.Interfaces.Sms;
using SiaAdmin.Infrastructure.Services;
using System.Net.Http.Headers; 
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SiaAdmin.Application.Interfaces.Mail;
using SiaAdmin.Application.Interfaces.SiaUser;
using SiaAdmin.Application.Interfaces.PushNotification;
using SiaAdmin.Application.Interfaces.BackgroundJob;
using SiaAdmin.Application.Interfaces.Database;
using SiaAdmin.Application.Interfaces.NotificationScheduler;
using SiaAdmin.Application.Interfaces.NotificationProcessor;
using SiaAdmin.Application.Interfaces.QueryBuilder;
using SiaAdmin.Application.Interfaces.Report;
using SiaAdmin.Infrastructure.Data;
using SiaAdmin.Infrastructure.Query;

namespace SiaAdmin.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
	        services.AddSingleton<IDataTableExcelService, DataTableExcelService>();
			services.AddSingleton<IExcelService, ExcelService>();
            services.AddSingleton<IConvertExcelFile, ConvertExcelFileService>();
            services.AddSingleton<ISmsService, SmsService>();
            services.AddSingleton<ISiaUserService, SiaUserService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IPushNotificationService, FirebasePushNotificationService>();
            services.AddTransient<IBackgroundJobService, HangfireBackgroundJobService>();
            services.AddTransient<INotificationSchedulerService, NotificationSchedulerService>();
            services.AddTransient<INotificationProcessor, NotificationProcessor>();
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

            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));
            services.AddScoped<IDatabaseService, SqlDatabaseService>();
            services.AddScoped<IReportService, ReportService>(); 
            services.AddScoped<IQueryBuilder, QueryBuilder>();
            services.AddScoped<IQueryTypeBuilder, TanismaAnketiQueryBuilder>();
            // Hangfire Server
            services.AddHangfireServer(options =>
            {
                options.WorkerCount = Configuration.HangfireWorkerCount;
                options.Queues = Configuration.HangfireQueues;
                options.ServerName = Configuration.HangfireServerName;
            });

           
        }

    }
}
