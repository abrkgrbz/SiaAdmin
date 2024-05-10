using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public partial class BlockList : BaseEntity
    {

        public int? RecType { get; set; }

        public string? Data { get; set; }

        public string? Note { get; set; }

        public int? Active { get; set; }

        public DateTime? Timestamp { get; set; }

        public int? IptalKodu { get; set; }
    }
}
