using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.DTOs.NotificationDTO;

namespace SiaAdmin.Application.Interfaces.PushNotification
{
    public interface IPushNotificationService
    {
        Task<bool> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string> data = null);
        Task<bool> SendNotificationToTopicAsync(string topic, string title, string body, Dictionary<string, string> data = null);
        Task<bool> SendBulkNotificationAsync(List<string> deviceTokens, string title, string body, Dictionary<string, string> data = null);

        Task<NotificationResult> SendNotificationDetailedAsync(string deviceToken, string title, string body, Dictionary<string, string> data = null);
        Task<NotificationResult> SendNotificationToTopicDetailedAsync(string topic, string title, string body, Dictionary<string, string> data = null);
        Task<NotificationResult> SendBulkNotificationDetailedAsync(List<string> deviceTokens, string title, string body, Dictionary<string, string> data = null);


    }
}
