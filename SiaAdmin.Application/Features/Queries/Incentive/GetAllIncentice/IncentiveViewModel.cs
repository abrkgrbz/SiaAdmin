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
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageWithCSS { get; set; }
        public int ShowInDisplay { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
