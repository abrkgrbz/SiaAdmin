using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Repositories.NotificationScheduledDeviceTokens
{
    public interface INotificationScheduledDeviceTokensReadRepository:IReadRepository<Domain.Entities.Models.NotificationScheduledDeviceTokens>
    {
        Task<IEnumerable<Domain.Entities.Models.NotificationScheduledDeviceTokens>> GetByNotificationHistoryId(
            int notificationHistoryId);
    }
}
