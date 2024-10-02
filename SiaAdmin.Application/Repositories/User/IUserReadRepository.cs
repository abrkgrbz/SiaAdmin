using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Custom;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Repositories
{
    public interface IUserReadRepository : IReadRepository<User>
    {
        List<Guid> ConvertInternalGuid(List<Guid> userGuids);
        bool IfExistUser(string msisdn);

        List<UserSurvey> GetUserSurveyList(Guid surveyUserGUID);
        UserSurveyPoints GetUserSurveyPoints(Guid surveyUserGUID);
        List<UserSelectIncentive> GetUserSelectIncentiveList(Guid surveyUserGUID);
        List<UserRecievedGifts> GetUserRecievedGiftsList(Guid surveyUserGUID);
        List<UserTransactionLogs> GetUserTransactionLogsList(Guid surveyUserGUID);
        UserProfile GetUserProfile(Guid surveyUserGUID);
        List<UserSurveyInfo> GetUserSurveyInfo(Guid surveyUserGUID);
        Task<List<LastSeenAdet>> GetListLastSeenAdet();
        Task<List<LastSeenSaat>> GetListLastSeenSaat();
    }
}
