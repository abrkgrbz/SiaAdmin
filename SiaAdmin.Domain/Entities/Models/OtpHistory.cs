using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public partial class OtpHistory : BaseEntity
    {

        public string? Msisdn { get; set; }

        public string? Otp { get; set; }

        public DateTime Timestamp { get; set; }

        public int? Status { get; set; }

        public string? LastIp { get; set; }

        public string? LastBrowser { get; set; }

        public string? TrackingId { get; set; }
        public int Source { get; set; }
    }
}
