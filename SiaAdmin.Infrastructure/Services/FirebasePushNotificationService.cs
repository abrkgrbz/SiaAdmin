using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SiaAdmin.Application.Interfaces.PushNotification;

namespace SiaAdmin.Infrastructure.Services
{
    public class FirebasePushNotificationService:IPushNotificationService
    {
        private readonly ILogger<FirebasePushNotificationService> _logger;
        private readonly FirebaseMessaging _firebaseMessaging;
        public FirebasePushNotificationService(  ILogger<FirebasePushNotificationService> logger)
        {
            _logger = logger;
            _firebaseMessaging = FirebaseMessaging.DefaultInstance;
        }


        public async Task<bool> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string> data = null)
        {
            try
            {
                var message = new Message()
                {
                    Token = deviceToken,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Data = data ?? new Dictionary<string, string>()
                };

                var response = await _firebaseMessaging.SendAsync(message);
                _logger.LogInformation($"Successfully sent message: {response}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending notification to device: {deviceToken}");
                return false;
            }
        }

        public async Task<bool> SendNotificationToTopicAsync(string topic, string title, string body, Dictionary<string, string> data = null)
        {
            try
            {
                var message = new Message()
                {
                    Topic = topic,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Data = data ?? new Dictionary<string, string>()
                };

                var response = await _firebaseMessaging.SendAsync(message);
                _logger.LogInformation($"Successfully sent message to topic {topic}: {response}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending notification to topic: {topic}");
                return false;
            }
        }

        public async Task<bool> SendBulkNotificationAsync(List<string> deviceTokens, string title, string body, Dictionary<string, string> data = null)
        {
            try
            {
                if (deviceTokens == null || deviceTokens.Count == 0)
                {
                    _logger.LogWarning("No device tokens provided for bulk notification");
                    return false;
                }

                // Firebase sınırlaması: Maksimum 500 alıcı
                const int maxBatchSize = 500;
                var success = true;

                for (int i = 0; i < deviceTokens.Count; i += maxBatchSize)
                {
                    var batch = deviceTokens.GetRange(
                        i, Math.Min(maxBatchSize, deviceTokens.Count - i));

                    var messages = new MulticastMessage()
                    {
                        Tokens = batch,
                        Notification = new Notification
                        {
                            Title = title,
                            Body = body
                        },
                        Data = data ?? new Dictionary<string, string>()
                    };

                    var response = await _firebaseMessaging.SendEachForMulticastAsync(messages);

                    if (response.FailureCount > 0)
                    {
                        success = false;
                        _logger.LogWarning($"Bulk notification partial failure: {response.FailureCount} failed, {response.SuccessCount} succeeded");
                    }
                    else
                    {
                        _logger.LogInformation($"Successfully sent {response.SuccessCount} messages in bulk");
                    }
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending bulk notifications");
                return false;
            }
        }
    }
}
