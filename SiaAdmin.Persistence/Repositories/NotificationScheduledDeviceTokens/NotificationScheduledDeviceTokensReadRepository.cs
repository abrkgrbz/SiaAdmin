using SiaAdmin.Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories.NotificationScheduledDeviceTokens;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.NotificationScheduledDeviceTokens
{
    public class NotificationScheduledDeviceTokensReadRepository:ReadRepository<Domain.Entities.Models.NotificationScheduledDeviceTokens>, INotificationScheduledDeviceTokensReadRepository
    {
        private DbSet<Domain.Entities.Models.NotificationScheduledDeviceTokens> _notificationScheduledDeviceTokens;
        public NotificationScheduledDeviceTokensReadRepository(SiaAdminDbContext context) : base(context)
        {
            _notificationScheduledDeviceTokens =
                context.Set<Domain.Entities.Models.NotificationScheduledDeviceTokens>();
        }

        public async Task<IEnumerable<Domain.Entities.Models.NotificationScheduledDeviceTokens>> GetByNotificationHistoryId(int notificationHistoryId)
        {
            return await _notificationScheduledDeviceTokens
                .Where(dt => dt.NotificationHistoryId == notificationHistoryId)
                .ToListAsync();
        }
    }
}
