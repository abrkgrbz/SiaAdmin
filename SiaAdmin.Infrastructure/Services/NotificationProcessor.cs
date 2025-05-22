using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SiaAdmin.Application.Interfaces.BackgroundJob;
using SiaAdmin.Application.Interfaces.NotificationProcessor;
using SiaAdmin.Application.Interfaces.PushNotification;
using SiaAdmin.Application.Repositories.NotificationHistory;
using SiaAdmin.Application.Repositories.NotificationScheduledDeviceTokens;

namespace SiaAdmin.Infrastructure.Services
{

    public class NotificationProcessor:INotificationProcessor
    {
        private readonly INotificationHistoryReadRepository _notificationHistoryReadRepository;
        private readonly INotificationHistoryWriteRepository _notificationHistoryWriteRepository;
        private readonly INotificationScheduledDeviceTokensReadRepository _notificationScheduledDeviceTokensReadRepository;
        private readonly INotificationScheduledDeviceTokensWriteRepository _notificationScheduledDeviceTokensWriteRepository;
        private readonly IPushNotificationService _pushNotificationService;
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly ILogger<NotificationProcessor> _logger;

        public NotificationProcessor(INotificationHistoryReadRepository notificationHistoryReadRepository, INotificationHistoryWriteRepository notificationHistoryWriteRepository, INotificationScheduledDeviceTokensReadRepository notificationScheduledDeviceTokensReadRepository, INotificationScheduledDeviceTokensWriteRepository notificationScheduledDeviceTokensWriteRepository, IPushNotificationService pushNotificationService, IBackgroundJobService backgroundJobService, ILogger<NotificationProcessor> logger)
        {
            _notificationHistoryReadRepository = notificationHistoryReadRepository;
            _notificationHistoryWriteRepository = notificationHistoryWriteRepository;
            _notificationScheduledDeviceTokensReadRepository = notificationScheduledDeviceTokensReadRepository;
            _notificationScheduledDeviceTokensWriteRepository = notificationScheduledDeviceTokensWriteRepository;
            _pushNotificationService = pushNotificationService;
            _backgroundJobService = backgroundJobService;
            _logger = logger;
        }

        /// <summary>
        /// Bekleyen tüm bildirimleri kontrol eder ve gönderilmesi gerekenleri işleme alır
        /// </summary>
        public async Task ProcessPendingNotifications()
        {
            try
            {
                // Pending durumundaki ve zamanı gelmiş olan bildirimleri al
                var pendingNotifications = await _notificationHistoryReadRepository
                    .GetPendingNotificationsToSend(DateTime.UtcNow);

                if (pendingNotifications == null || !pendingNotifications.Any())
                {
                    // Bekleyen bildirim yoksa işlem yapma
                    return;
                }

                foreach (var notification in pendingNotifications)
                {
                    // Durumu güncelle
                    notification.Status = 2;
                    notification.ScheduleStatus = "Processing";
                    notification.UpdatedAt = DateTime.UtcNow;
                    _notificationHistoryWriteRepository.Update(notification);
                     
                    _backgroundJobService.Enqueue<INotificationProcessor>(
                        processor => processor.ProcessNotification(notification.Id));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bekleyen bildirimler kontrol edilirken hata oluştu");
            }
        }

        /// <summary>
        /// Belirli bir bildirimi işler ve gönderir
        /// </summary>
        public async Task ProcessNotification(int notificationId)
        {
            var notification = await _notificationHistoryReadRepository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                _logger.LogWarning($"İşlenecek bildirim bulunamadı: {notificationId}");
                return;
            }

            try
            {
                // Bildirim durumunu kontrol et
                if (notification.Status != 2)
                {
                    _logger.LogWarning($"Bildirim işlenmeye uygun durumda değil: {notificationId}, Durum: {notification.Status}");
                    return;
                }

                // İlgili token kayıtlarını al
                var deviceTokens = await _notificationScheduledDeviceTokensReadRepository
                    .GetByNotificationHistoryId(notificationId);

                if (deviceTokens == null || !deviceTokens.Any())
                {
                    _logger.LogWarning($"Bildirim için cihaz token bulunamadı: {notificationId}");

                    notification.Status = 0;
                    notification.SentAt = DateTime.UtcNow;
                    notification.ScheduleStatus = "Completed";
                    notification.UpdatedAt = DateTime.UtcNow;
                    _notificationHistoryWriteRepository.Update(notification);
                    return;
                }

                // Pending durumunda olan token'ları filtrele
                var pendingTokens = deviceTokens
                    .Where(dt => dt.Status == "Pending")
                    .ToList();

                if (!pendingTokens.Any())
                {
                    _logger.LogWarning($"Bildirim için bekleyen cihaz token kalmamış: {notificationId}");
                    notification.Status = 0;
                    notification.SentAt = DateTime.UtcNow;
                    notification.ScheduleStatus = "Completed";
                    notification.UpdatedAt = DateTime.UtcNow;
                    _notificationHistoryWriteRepository.Update(notification);
                    return;
                }

                // FCM'ye gönderilecek verileri hazırla
                var tokenList = pendingTokens
                    .Select(dt => dt.DeviceToken)
                    .ToList();

                // Additional data hazırla
                Dictionary<string, string> additionalData = new Dictionary<string, string>();

                // Toplu bildirim gönderimi yap (detailed versiyonu kullan)
                var notificationResult = await _pushNotificationService.SendBulkNotificationDetailedAsync(
                    tokenList,
                    notification.NotificationTitle,
                    notification.NotificationContent,
                    additionalData);

                // Sonuçları kullan
                bool success = notificationResult.Success;
                int successCount = notificationResult.SuccessCount;
                int failCount = notificationResult.FailureCount;

                // Token durumlarını güncelle
                foreach (var token in pendingTokens)
                {
                    token.Status = success ? "Delivered" : "Failed";
                    token.DeliveryAttemptedAt = DateTime.UtcNow;
                    token.DeliveryResult = success ? "Success" : "Failed";
                    token.UpdatedAt = DateTime.UtcNow;

                    _notificationScheduledDeviceTokensWriteRepository.Update(token);
                }

                // Bildirim durumunu güncelle
                notification.Status = success ? 0 : 1;
                notification.ScheduleStatus = "Completed";
                notification.SentAt = DateTime.UtcNow;
                notification.SuccessfulDeliveryCount = successCount;
                notification.FailedDeliveryCount = failCount;
                notification.UpdatedAt = DateTime.UtcNow;
                notification.ResponsePayload = notificationResult.Payload; // Payload'ı kaydet

                _notificationHistoryWriteRepository.Update(notification);

                _logger.LogInformation($"Bildirim başarıyla işlendi. NotificationId: {notificationId}, " +
                                     $"Başarılı: {successCount}, Başarısız: {failCount}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Bildirim işlenirken hata: {notificationId}");

                notification.Status = 1;
                notification.ErrorCode = "500";
                notification.ErrorMessage = ex.Message;
                notification.UpdatedAt = DateTime.UtcNow;
                notification.ScheduleStatus = "Failed";
                notification.ResponsePayload = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Error = new
                    {
                        Type = ex.GetType().Name,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    }
                }, Newtonsoft.Json.Formatting.Indented);

                _notificationHistoryWriteRepository.Update(notification);

                // Yeniden deneme yapmak isterseniz
                if (notification.RetryCount < 3) // maksimum 3 deneme
                {
                    notification.RetryCount++;
                    notification.Status = 2;
                    notification.ScheduleStatus = "Scheduled";
                    _notificationHistoryWriteRepository.Update(notification);

                    _backgroundJobService.Schedule<INotificationProcessor>(
                        processor => processor.ProcessNotification(notificationId),
                        TimeSpan.FromMinutes(15));
                }
            }
        }
    }
}
