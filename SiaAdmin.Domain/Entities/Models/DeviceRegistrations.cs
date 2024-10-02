using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public class DeviceRegistrations:BaseEntity
    {
        public string DeviceIdToken { get; set; } 
        public Guid InternalGUID { get; set; } 
        public DateTime TimeStamp { get; set; }
        public bool Active { get; set; }
    }
}
