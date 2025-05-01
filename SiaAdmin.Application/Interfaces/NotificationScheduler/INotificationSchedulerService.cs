using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Interfaces.NotificationScheduler
{
    public interface INotificationSchedulerService
    {
        Task<string> ScheduleNotification(string deviceToken, string title, string body,
            Dictionary<string, string> data, DateTime scheduledTime);

        Task<string> ScheduleTopicNotification(string topic, string title, string body,
            Dictionary<string, string> data, DateTime scheduledTime);

        Task<string> ScheduleRecurringNotification(string topic, string title, string body,
            Dictionary<string, string> data, string cronExpression);

        Task CancelScheduledNotification(string jobId);
    }
}
