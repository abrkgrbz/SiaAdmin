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

        public DateTime Timestamp { get; set; }
    }
}
