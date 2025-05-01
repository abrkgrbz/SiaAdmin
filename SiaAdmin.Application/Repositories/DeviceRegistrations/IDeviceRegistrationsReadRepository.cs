using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Repositories.DeviceRegistrations
{
    public interface IDeviceRegistrationsReadRepository:IReadRepository<Domain.Entities.Models.DeviceRegistrations>
    {
        List<string> GetDeviceIdTokensBySurveyId(int surveyId);
        List<string> GetDeviceTokensNotInSurvey(int surveyId);
        Task<int> GetDeviceTokenIdsCount(int surveyId);
        Task<List<string>> GetDeviceTokenIdsBySurveyId(int surveyId);
    }
}
