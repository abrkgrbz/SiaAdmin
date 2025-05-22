using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.Dashboard.GetProfileLifetimeStats
{
    public class GetProfileLifetimeStatsResponse
    {
        public string LastSeen { get; set; }
        public double ProfilYasamSaatDegeri { get; set; }
    }
}
