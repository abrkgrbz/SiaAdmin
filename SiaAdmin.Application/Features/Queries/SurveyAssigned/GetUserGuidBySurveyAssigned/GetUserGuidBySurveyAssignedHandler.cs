using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.SurveyAssigned.GetUserGuidBySurveyAssigned
{
    public class GetUserGuidBySurveyAssignedHandler:IRequestHandler<GetUserGuidBySurveyAssignedRequest,GetUserGuidBySurveyAssignedResponse>
    {
        private readonly ISurveyAssignedReadRepository _surveyAssignedReadRepository;

        public GetUserGuidBySurveyAssignedHandler(ISurveyAssignedReadRepository surveyAssignedReadRepository)
        {
            _surveyAssignedReadRepository = surveyAssignedReadRepository;
        }

        public async Task<GetUserGuidBySurveyAssignedResponse> Handle(GetUserGuidBySurveyAssignedRequest request, CancellationToken cancellationToken)
        {
            var data = _surveyAssignedReadRepository.GetWhere(x => x.SurveyId == request.Id, false).ToList();
            GetUserGuidBySurveyAssignedResponse response = new GetUserGuidBySurveyAssignedResponse();
            foreach (var surveyAssigned in data)
            {
                response.InternalGUIDs.Add(surveyAssigned.InternalGuid);
            }

            return new GetUserGuidBySurveyAssignedResponse() { InternalGUIDs = response.InternalGUIDs };
        }

    }
}
