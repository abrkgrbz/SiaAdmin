using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Repositories.NotificationHistory;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.NotificationHistory
{
    public class NotificationHistoryWriteRepository:WriteRepository<Domain.Entities.Models.NotificationHistory>,INotificationHistoryWriteRepository
    {
        public NotificationHistoryWriteRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
