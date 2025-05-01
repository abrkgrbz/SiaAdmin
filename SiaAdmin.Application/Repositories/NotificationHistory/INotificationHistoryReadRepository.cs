using SiaAdmin.Domain.Entities.Procedure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Repositories.NotificationHistory
{
    public interface INotificationHistoryReadRepository:IReadRepository<Domain.Entities.Models.NotificationHistory>
    {
        Task<NotificationCooldownResult> CheckNotificationCooldownAsync(int surveyId, int cooldownHours = 6);
        Task<IEnumerable<Domain.Entities.Models.NotificationHistory>> GetPendingNotificationsToSend(DateTime currentTime);

    }
}
