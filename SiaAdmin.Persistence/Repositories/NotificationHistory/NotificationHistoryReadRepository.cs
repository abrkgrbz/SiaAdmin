using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Repositories.NotificationHistory;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Domain.Entities.Procedure;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.NotificationHistory
{
    public class NotificationHistoryReadRepository:ReadRepository<Domain.Entities.Models.NotificationHistory>, INotificationHistoryReadRepository
    {
        private readonly IStoredProcedureRepository<NotificationCooldownResult> _spRepository;

        private readonly DbSet<Domain.Entities.Models.NotificationHistory> _notificationHistories;
        public NotificationHistoryReadRepository(SiaAdminDbContext context, IStoredProcedureRepository<NotificationCooldownResult> spRepository) : base(context)
        {
            _spRepository = spRepository;
            _notificationHistories = context.Set<Domain.Entities.Models.NotificationHistory>();
        }

        public async Task<NotificationCooldownResult> CheckNotificationCooldownAsync(int surveyId, int cooldownHours = 6)
        {
            var parameters = new[]
            {
                new SqlParameter { ParameterName = "@SurveyId", Value = surveyId },
                new SqlParameter { ParameterName = "@CooldownHours", Value = cooldownHours }
            };

            var result = await _spRepository.ExecuteStoredProcedureAsync("sp_CheckNotificationCooldown", parameters);
            return result.AsEnumerable().FirstOrDefault();
        }

        public async Task<IEnumerable<Domain.Entities.Models.NotificationHistory>> GetPendingNotificationsToSend(DateTime currentTime)
        {
            return await _notificationHistories.Where(n =>
                    n.Status == 2 &&
                    n.ScheduleStatus == "Pending" &&
                    (n.ScheduledFor == null || n.ScheduledFor <= currentTime))
                .ToListAsync();
        }
    }
}
