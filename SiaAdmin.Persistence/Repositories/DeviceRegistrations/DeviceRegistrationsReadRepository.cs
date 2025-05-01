using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories.DeviceRegistrations;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.DeviceRegistrations
{
    public class DeviceRegistrationsReadRepository : ReadRepository<Domain.Entities.Models.DeviceRegistrations>, IDeviceRegistrationsReadRepository
    {
        private DbSet<SurveyLog> _surveyLog;
        private DbSet<Domain.Entities.Models.DeviceRegistrations> _deviceRegistrations;
        private DbSet<SurveyAssigned> _surveyAssigned;
        private DbSet<User> _user;
        public DeviceRegistrationsReadRepository(SiaAdminDbContext context) : base(context)
        {
            _surveyLog = context.Set<SurveyLog>();
            _deviceRegistrations = context.Set<Domain.Entities.Models.DeviceRegistrations>();
            _user = context.Set<User>();
            _surveyAssigned = context.Set<SurveyAssigned>();
        }


        public List<string> GetDeviceIdTokensBySurveyId(int surveyId)
        {
            var internalGuids = _surveyLog
                .Where(sl => sl.SurveyId == surveyId && sl.Approved == 0)
                .Join(_user,
                    sl => sl.SurveyUserGuid,
                    u => u.SurveyUserGuid,
                    (sl, u) => u.InternalGuid);

            var result = _deviceRegistrations
                .Where(dr => internalGuids.Contains(dr.InternalGUID))
                .GroupBy(dr => new { dr.DeviceIdToken, dr.InternalGUID })
                .Select(g => g.Key.DeviceIdToken)
                .ToList();

            return result;
        }

        public List<string> GetDeviceTokensNotInSurvey(int surveyId)
        {
            var result = _deviceRegistrations
                .Where(dr =>
                    !_surveyLog
                        .Where(sl => sl.SurveyId == surveyId)
                        .Join(_user,
                            sl => sl.SurveyUserGuid,
                            u => u.SurveyUserGuid,
                            (sl, u) => u.InternalGuid)
                        .Contains(dr.InternalGUID)
                    &&
                    _surveyLog
                        .Where(sl => sl.SurveyId == 2 && sl.Active == 1 && sl.Approved == 1)
                        .Join(_user,
                            sl => sl.SurveyUserGuid,
                            u => u.SurveyUserGuid,
                            (sl, u) => new { InternalGUID = u.InternalGuid, UserActive = u.Active })
                        .Where(result => result.UserActive == 1)
                        .Select(result => result.InternalGUID)
                        .Contains(dr.InternalGUID)
                )
                .Select(dr => dr.DeviceIdToken)
                .Distinct()
                .ToList();

            return result;
        }

        public async Task<int> GetDeviceTokenIdsCount(int surveyId)
        {
            var assignedUsers = await _surveyAssigned
                .Where(sa => sa.SurveyId == surveyId)
                .Select(sa => sa.InternalGuid)
                .ToListAsync();

            var activeUsers = await _user
                .Join(_surveyLog,
                    u => u.SurveyUserGuid,
                    sl => sl.SurveyUserGuid,
                    (u, sl) => new { User = u, SurveyLog = sl })
                .Where(joined => joined.SurveyLog.SurveyId == surveyId &&
                                 (joined.SurveyLog.Active == 1 || joined.SurveyLog.Active == 2))
                .Select(joined => joined.User.InternalGuid)
                .ToListAsync();

            var deviceTokensCount = await _deviceRegistrations
                .Where(dr => assignedUsers.Contains(dr.InternalGUID) &&
                             !activeUsers.Contains(dr.InternalGUID))
                .Select(dr => dr.DeviceIdToken)
                .Distinct()
                .CountAsync();

            return deviceTokensCount;
        }

        public async Task<List<string>> GetDeviceTokenIdsBySurveyId(int surveyId)
        {
            var assignedUsers = await _surveyAssigned
                .Where(sa => sa.SurveyId == surveyId)
                .Select(sa => sa.InternalGuid)
                .ToListAsync();

            var activeUsers = await _user
                .Join(_surveyLog,
                    u => u.SurveyUserGuid,
                    sl => sl.SurveyUserGuid,
                    (u, sl) => new { User = u, SurveyLog = sl })
                .Where(joined => joined.SurveyLog.SurveyId == surveyId &&
                                 (joined.SurveyLog.Active == 1 || joined.SurveyLog.Active == 2))
                .Select(joined => joined.User.InternalGuid)
                .ToListAsync();

            var deviceTokens = await _deviceRegistrations
                .Where(dr => assignedUsers.Contains(dr.InternalGUID) &&
                             !activeUsers.Contains(dr.InternalGUID))
                .Select(dr => dr.DeviceIdToken)
                .Distinct()
                .ToListAsync();

            return deviceTokens;
        }
    }
}
