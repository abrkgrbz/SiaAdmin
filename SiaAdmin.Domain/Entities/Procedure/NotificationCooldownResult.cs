using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Domain.Entities.Procedure
{
    [Keyless]
    public class NotificationCooldownResult
    { 
        public bool CanSendNotification { get; set; }
        public string Message { get; set; }
        public DateTime? LastNotificationSent { get; set; }
        public int? LastNotificationStatus { get; set; }
    }
}
