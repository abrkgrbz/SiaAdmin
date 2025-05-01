using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.NotificationHistory.CheckNotificationCooldown
{
    public class CheckNotificationCooldownResponse
    {
        [JsonPropertyName("canSend")]
        public bool CanSendNotification { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonIgnore] // Bunu doğrudan serialize etmeyeceğiz
        public DateTime? LastNotificationSent { get; set; }

        [JsonPropertyName("lastSent")]
        public string LastSent => LastNotificationSent?.ToString("dd.MM.yyyy HH:mm");

        [JsonIgnore]
        public int LastNotificationStatus { get; set; }
         

    }
}
