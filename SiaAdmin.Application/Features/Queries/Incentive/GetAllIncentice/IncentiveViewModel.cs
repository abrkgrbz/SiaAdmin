using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.Incentive.GetAllIncentice
{
    public class IncentiveViewModel
    {

        public string IncentiveText { get; set; } = null!;

        public int Points { get; set; }

        public int Active { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
