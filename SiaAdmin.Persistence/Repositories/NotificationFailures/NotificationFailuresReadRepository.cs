using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories.NotificationFailures;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.NotificationFailures
{
    public class NotificationFailuresReadRepository:ReadRepository<NotificationFailure>,INotificationFailuresReadRepository
    {
        public NotificationFailuresReadRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
