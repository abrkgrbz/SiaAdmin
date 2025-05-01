using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories.NotificationFailures;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.NotificationFailures
{
    public class NotificationFailuresWriteRepository:WriteRepository<NotificationFailure>,INotificationFailuresWriteRepository
    {
        
        public NotificationFailuresWriteRepository(SiaAdminDbContext context) : base(context)
        {
            
        }
    }
}
