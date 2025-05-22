using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.Dashboard.GetPassiveUserStats
{
    public class GetPassiveUserStatsResponse
    {
        public string LastSeen { get; set; }
        public int Adet { get; set; }
    }
}
