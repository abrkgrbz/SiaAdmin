using SiaAdmin.Application.Interfaces.BackgroundJob;
using SiaAdmin.Application.Interfaces.NotificationScheduler;
using SiaAdmin.Application.Interfaces.PushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Infrastructure.Services
{
    public class NotificationSchedulerService: INotificationSchedulerService
    {
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly IPushNotificationService _pushNotificationService;

        public NotificationSchedulerService(IBackgroundJobService backgroundJobService, IPushNotificationService pushNotificationService)
        {
            _backgroundJobService = backgroundJobService;
            _pushNotificationService = pushNotificationService;
        }

        public Task<string> ScheduleNotification(string deviceToken, string title, string body,
            Dictionary<string, string> data, DateTime scheduledTime)
        {
            string jobId = _backgroundJobService.Schedule<IPushNotificationService>(
                service => service.SendNotificationAsync(deviceToken, title, body, data),
                scheduledTime - DateTime.Now);

            return Task.FromResult(jobId);
        }

        public Task<string> ScheduleTopicNotification(string topic, string title, string body,
            Dictionary<string, string> data, DateTime scheduledTime)
        {
            string jobId = _backgroundJobService.Schedule<IPushNotificationService>(
                service => service.SendNotificationToTopicAsync(topic, title, body, data),
                scheduledTime - DateTime.Now);

            return Task.FromResult(jobId);
        }

        public Task<string> ScheduleRecurringNotification(string topic, string title, string body,
            Dictionary<string, string> data, string cronExpression)
        {
            string jobId = $"notification_{Guid.NewGuid()}";

            _backgroundJobService.RecurringJob<IPushNotificationService>(
                jobId,
                service => service.SendNotificationToTopicAsync(topic, title, body, data),
                cronExpression);

            return Task.FromResult(jobId);
        }

        public Task CancelScheduledNotification(string jobId)
        {
            _backgroundJobService.Delete(jobId);
            return Task.CompletedTask;
        }
    }
}
