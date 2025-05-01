    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Enums;

namespace SiaAdmin.Application.Features.Commands.NotificationHistory.CreateNotificationHistory
{
    public class NotificationHistoryRequest:IRequest<NotificationHistoryResponse>
    {
        public int SurveyId { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public string SentBy { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationContent { get; set; }
        public int ReciptientCount { get; set; }
        public int Status { get; set; } =  (int)NotificationStatus.Pending;
        public DateTime ScheduledFor { get; set; }
        public NotificationStatus ScheduleStatus { get; set; } = NotificationStatus.Pending;
        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;

    }
}
