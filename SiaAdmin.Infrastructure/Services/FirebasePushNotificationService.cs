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
using SiaAdmin.Application.DTOs.NotificationDTO;

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

        // Yeni detaylı metotlar ekleyelim
        public async Task<NotificationResult> SendBulkNotificationDetailedAsync(List<string> deviceTokens, string title, string body, Dictionary<string, string> data = null)
        {
            var result = new NotificationResult
            {
                Success = true,
                TotalCount = deviceTokens?.Count ?? 0,
                SuccessCount = 0,
                FailureCount = 0
            };

            try
            {
                if (deviceTokens == null || deviceTokens.Count == 0)
                {
                    _logger.LogWarning("No device tokens provided for bulk notification");
                    result.Success = false;
                    return result;
                }

                // Tüm yanıtları toplamak için liste
                var allResponses = new List<object>();

                // Firebase sınırlaması: Maksimum 500 alıcı
                const int maxBatchSize = 500;

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

                    // Toplamları güncelle
                    result.SuccessCount += response.SuccessCount;
                    result.FailureCount += response.FailureCount;

                    // Başarısız bildirimler varsa başarı durumunu güncelle
                    if (response.FailureCount > 0)
                    {
                        result.Success = false;
                        _logger.LogWarning($"Bulk notification partial failure: {response.FailureCount} failed, {response.SuccessCount} succeeded");
                    }
                    else
                    {
                        _logger.LogInformation($"Successfully sent {response.SuccessCount} messages in bulk");
                    }

                    // Yanıtı listeye ekle
                    var batchResponse = new
                    {
                        BatchSize = batch.Count,
                        SuccessCount = response.SuccessCount,
                        FailureCount = response.FailureCount,
                        Responses = response.Responses.Select((r, index) => new
                        {
                            Token = batch[index],
                            Success = r.IsSuccess,
                            MessageId = r.MessageId,
                            Error = r.IsSuccess ? null : new
                            {
                                Code = r.Exception?.GetType().Name,
                                Message = r.Exception?.Message
                            }
                        }).ToList()
                    };

                    allResponses.Add(batchResponse);
                }

                // Tüm yanıtları JSON olarak serialize et
                result.Payload = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    TotalBatches = allResponses.Count,
                    TotalDevices = result.TotalCount,
                    TotalSuccess = result.SuccessCount,
                    TotalFailure = result.FailureCount,
                    Batches = allResponses
                }, Newtonsoft.Json.Formatting.Indented);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending bulk notifications");

                result.Success = false;
                result.Payload = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Error = new
                    {
                        Type = ex.GetType().Name,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    }
                }, Newtonsoft.Json.Formatting.Indented);

                return result;
            }
        }

        public async Task<NotificationResult> SendNotificationDetailedAsync(string deviceToken, string title, string body, Dictionary<string, string> data = null)
        {
            var result = new NotificationResult
            {
                Success = false,
                TotalCount = 1,
                SuccessCount = 0,
                FailureCount = 0
            };

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

                result.Success = true;
                result.SuccessCount = 1;
                result.Payload = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    MessageId = response,
                    Token = deviceToken,
                    Success = true
                }, Newtonsoft.Json.Formatting.Indented);

                _logger.LogInformation($"Successfully sent message: {response}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending notification to device: {deviceToken}");

                result.FailureCount = 1;
                result.Payload = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Token = deviceToken,
                    Success = false,
                    Error = new
                    {
                        Type = ex.GetType().Name,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    }
                }, Newtonsoft.Json.Formatting.Indented);

                return result;
            }
        }

        public async Task<NotificationResult> SendNotificationToTopicDetailedAsync(string topic, string title, string body, Dictionary<string, string> data = null)
        {
            var result = new NotificationResult
            {
                Success = false,
                TotalCount = 1, // Topic bildiriminde bu tahmini bir sayı olur
                SuccessCount = 0,
                FailureCount = 0
            };

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

                result.Success = true;
                result.SuccessCount = 1; // Topic bildiriminde başarılı sayısını bilemeyiz
                result.Payload = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    MessageId = response,
                    Topic = topic,
                    Success = true
                }, Newtonsoft.Json.Formatting.Indented);

                _logger.LogInformation($"Successfully sent message to topic {topic}: {response}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending notification to topic: {topic}");

                result.FailureCount = 1;
                result.Payload = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Topic = topic,
                    Success = false,
                    Error = new
                    {
                        Type = ex.GetType().Name,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    }
                }, Newtonsoft.Json.Formatting.Indented);

                return result;
            }
        }
    }
}
