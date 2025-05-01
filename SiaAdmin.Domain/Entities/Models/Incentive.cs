using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public partial class Incentive : BaseEntity
    {

        public string IncentiveText { get; set; } = null!;

        public int Points { get; set; }

        public int Active { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageWithCSS { get; set; }
        public int ShowInDisplay { get; set; }
        public int Test { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
