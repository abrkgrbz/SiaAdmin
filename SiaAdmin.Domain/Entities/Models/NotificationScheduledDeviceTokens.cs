using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public class NotificationScheduledDeviceTokens:BaseEntity
    { 
        public int NotificationHistoryId { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceType { get; set; }
        public string? UserId { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime? DeliveryAttemptedAt { get; set; }
        public string? DeliveryResult { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // İlişki
        public virtual NotificationHistory NotificationHistory { get; set; }
    }
}
