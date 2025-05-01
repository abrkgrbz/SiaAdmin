using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
   
    public class NotificationHistory:BaseEntity
    {
        public int SurveyId { get; set; }
        public DateTime SentAt { get; set; }
        public string SentBy { get; set; }
        public string NotificationTitle { get; set; } 
        public string NotificationContent { get; set; }
        public int? RecipientCount { get; set; } 
        [Required]
        public int Status { get; set; } = 2; 
        public string? ErrorCode { get; set; } 
        public string? ErrorMessage { get; set; } 
        public string? ResponsePayload { get; set; }
        public int? SuccessfulDeliveryCount { get; set; }
        public int? FailedDeliveryCount { get; set; }
        public int RetryCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime ScheduledFor { get; set; }
        public string ScheduleStatus { get; set; }
        public virtual ICollection<NotificationFailure> Failures { get; set; } = new List<NotificationFailure>();
        public virtual ICollection<NotificationScheduledDeviceTokens> DeviceTokens { get; set; }


    }
}
