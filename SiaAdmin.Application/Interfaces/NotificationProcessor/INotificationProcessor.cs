using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Interfaces.NotificationProcessor
{
    public interface INotificationProcessor
    {
        Task ProcessPendingNotifications();
        Task ProcessNotification(int notificationId);
    }
}
