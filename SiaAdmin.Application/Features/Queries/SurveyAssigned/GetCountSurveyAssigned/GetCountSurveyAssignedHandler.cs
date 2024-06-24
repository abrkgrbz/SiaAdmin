using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.SurveyAssigned.GetCountSurveyAssigned
{
    public class GetCountSurveyAssignedHandler:IRequestHandler<GetCountSurveyAssignedRequest,GetCountSurveyAssignedResponse>
    {
        private readonly ISurveyAssignedReadRepository _surveyAssignedReadRepository;

        public GetCountSurveyAssignedHandler(ISurveyAssignedReadRepository surveyAssignedReadRepository)
        {
            _surveyAssignedReadRepository = surveyAssignedReadRepository;
        }

        public async Task<GetCountSurveyAssignedResponse> Handle(GetCountSurveyAssignedRequest request, CancellationToken cancellationToken)
        {
            var count = _surveyAssignedReadRepository.GetWhere(x => x.SurveyId == request.Id).ToList().Count;
            return new GetCountSurveyAssignedResponse(){Count = count};
        }
    }
}
