using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SiaAdmin.Infrastructure
{
    public class Configuration
    {
        static public string FirebaseApiKey
        {
            get
            {
                ConfigurationManager configurationManager = new(); 
                configurationManager.AddJsonFile("appsettings.json");
                return configurationManager.GetValue<string>("Firebase:ApiKey");
            }
        }
        static public string FirebaseAudience
        {
            get
            {
                ConfigurationManager configurationManager = new();
                configurationManager.AddJsonFile("appsettings.json");
                return configurationManager.GetValue<string>("Authentication:Audience");
            }
        }

        static public string FirebaseAuthority
        {
            get
            {
                ConfigurationManager configurationManager = new();
                configurationManager.AddJsonFile("appsettings.json");
                return configurationManager.GetValue<string>("Authentication:ValidIssuer");
            }
        }


    }
}
