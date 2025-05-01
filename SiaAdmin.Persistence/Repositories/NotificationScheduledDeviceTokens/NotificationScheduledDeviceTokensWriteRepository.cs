using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories.NotificationScheduledDeviceTokens;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.NotificationScheduledDeviceTokens
{
    public class NotificationScheduledDeviceTokensWriteRepository:WriteRepository<Domain.Entities.Models.NotificationScheduledDeviceTokens>, INotificationScheduledDeviceTokensWriteRepository
    {
        public NotificationScheduledDeviceTokensWriteRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
