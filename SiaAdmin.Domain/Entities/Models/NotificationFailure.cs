using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public class NotificationFailure:BaseEntity
    { 
        public int NotificationHistoryId { get; set; }
        public Guid UserGuid { get; set; }
        public string DeviceToken { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime FailedAt { get; set; } = DateTime.Now;

        // Navigation property
        public virtual NotificationHistory NotificationHistory { get; set; }
    }
}
