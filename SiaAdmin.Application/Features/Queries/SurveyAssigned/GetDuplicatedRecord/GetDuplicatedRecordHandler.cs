using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
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
            var list = _surveyAssignedReadRepository.GetWhere(x => x.SurveyId == request.SurveyId, false)
                .GroupBy(x => new { x.InternalGuid, x.SurveyId })
                .Select(x => new DuplicatedRecordViewModel()
                {
                    SurveyId = x.Key.SurveyId,
                    InternalGuid = x.Key.InternalGuid
                }).ToList();
            if (list.Count > 0)
            {
                return new GetDuplicatedRecordResponse()
                {
                    DuplicatedRecordViewModels = list,
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
