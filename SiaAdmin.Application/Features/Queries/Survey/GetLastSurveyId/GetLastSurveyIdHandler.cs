using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.Survey.GetLastSurveyId
{
    public class GetLastSurveyIdHandler:IRequestHandler<GetLastSurveyIdRequest,GetLastSurveyIdResponse>
    {
        private ISurveyReadRepository _surveyReadRepository;

        public GetLastSurveyIdHandler(ISurveyReadRepository surveyReadRepository)
        {
            _surveyReadRepository = surveyReadRepository;
        }

        public async Task<GetLastSurveyIdResponse> Handle(GetLastSurveyIdRequest request, CancellationToken cancellationToken)
        {
            var result = await _surveyReadRepository.GetAll(false).ToListAsync();
            int lastRecordId=result.OrderByDescending(x => x.Id).First().Id;
            return new GetLastSurveyIdResponse { SurveyId=lastRecordId};
        }
    }
}
