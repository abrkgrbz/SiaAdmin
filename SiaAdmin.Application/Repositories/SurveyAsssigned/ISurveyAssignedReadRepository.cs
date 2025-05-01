using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Custom;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Repositories
{
    public interface ISurveyAssignedReadRepository:IReadRepository<SurveyAssigned>
    {
        bool CheckDuplicatedRecordByUserGUID(int surveyId,List<Guid> userGuidList);
        bool IsDuplicatedGuid(int surveyId, Guid internalGuid);

        Task<List<MukerreKayit>> GetDuplicatedRecordList(int surveyId);
    }
}
