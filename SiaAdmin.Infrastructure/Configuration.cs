using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Configuration;

namespace SiaAdmin.Infrastructure
{
    public class Configuration
    {
        private static readonly Lazy<IConfiguration> _configuration = new Lazy<IConfiguration>(() => {
            var configurationManager = new ConfigurationManager();
            configurationManager.AddJsonFile("appsettings.json");
            return configurationManager;
        });
 

        private static IConfiguration Config => _configuration.Value;

        public static string FirebaseApiKey => Config.GetValue<string>("Firebase:ApiKey");

        public static string FirebaseAudience => Config.GetValue<string>("Authentication:Audience");

        public static string FirebaseAuthority => Config.GetValue<string>("Authentication:ValidIssuer");

        public static string GetConnectionString => Config.GetConnectionString("HangfireConnection");

        public static int HangfireWorkerCount => Config.GetValue<int>("Hangfire:WorkerCount", 10);

        public static string[] HangfireQueues => Config.GetSection("Hangfire:Queues").Get<string[]>() ?? new[] { "default" };

        public static string HangfireServerName => $"Hangfire.Server.{Environment.MachineName}"; 

       
    }
}
