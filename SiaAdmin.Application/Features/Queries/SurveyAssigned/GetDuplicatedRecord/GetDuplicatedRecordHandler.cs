using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.SurveyAssigned.GetDuplicatedRecord
{
    public class GetDuplicatedRecordHandler : IRequestHandler<GetDuplicatedRecordRequest, GetDuplicatedRecordResponse>
    {
        private readonly ISurveyAssignedReadRepository _surveyAssignedReadRepository;

        public GetDuplicatedRecordHandler(ISurveyAssignedReadRepository surveyAssignedReadRepository)
        {
            _surveyAssignedReadRepository = surveyAssignedReadRepository;
        }

        public async Task<GetDuplicatedRecordResponse> Handle(GetDuplicatedRecordRequest request, CancellationToken cancellationToken)
        {
            var list =await _surveyAssignedReadRepository.GetDuplicatedRecordList(request.SurveyId);
             
            if (list.Count > 0)
            {
                return new GetDuplicatedRecordResponse()
                { 
                    Count = list.Count,
                    Message = "Mükerrer kayıtlar bulundu!"
                };
            }
            else
            {
                return new GetDuplicatedRecordResponse()
                {
                    Message = "Mükkerrer kayıt bulunamadı"
                };
            }
        }
    }
}
