using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.WaitData.GetSummaryWaitData
{
    public class GetSummaryWaitDataHandler:IRequestHandler<GetSummaryWaitDataRequest,GetSummaryWaitDataResponse>
    {
        private readonly IWaitDataReadRepository _waitDataReadRepository;

        public GetSummaryWaitDataHandler(IWaitDataReadRepository waitDataReadRepository)
        {
            _waitDataReadRepository = waitDataReadRepository;
        }

        public async Task<GetSummaryWaitDataResponse> Handle(GetSummaryWaitDataRequest request, CancellationToken cancellationToken)
        {
            var waitData = _waitDataReadRepository.GetAll(false)
                .GroupBy(x => new { x.SurveyId, x.Tarih.Date })
                .Select(x => new
                {
                    SurveyId=x.Key.SurveyId,
                    Tarih= x.Key.Date,
                    Adet = x.Count()
                })
                .OrderBy(x=>x.Tarih).ToList();

            return new GetSummaryWaitDataResponse() { data = waitData };
             
        }
    }
}
