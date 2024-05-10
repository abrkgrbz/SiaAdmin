using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace SiaAdmin.Persistence
{
    static class Configuration
    {
        static public string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new(); 
                 
                configurationManager.AddJsonFile("appsettings.json");
                return configurationManager.GetConnectionString("SiaAdminSql");
            }
        }
    }
}
