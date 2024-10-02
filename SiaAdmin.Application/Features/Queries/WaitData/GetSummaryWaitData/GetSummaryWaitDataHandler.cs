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
            var waitData =await _waitDataReadRepository.GetAllWaitData();
            var waitDataQueryable = waitData.AsQueryable();
            int recordsFiltered = 0, recordTotal = 0;
            if (!string.IsNullOrEmpty(request.searchValue))
            {
                waitDataQueryable = waitDataQueryable.Where(x => x.SurveyId.ToString().ToLower().Contains(request.searchValue.ToLower())
                                               || x.SurveyId.ToString().Equals(request.searchValue));
            }

            if (!string.IsNullOrEmpty(request.orderColumnName) && !string.IsNullOrEmpty(request.orderDir))
            {
                 
                recordsFiltered = waitDataQueryable.Count();
                recordTotal = waitDataQueryable.Count();

            }
            var waitDatas = waitDataQueryable.Skip(request.Start).Take(request.Length).ToList();
            return new GetSummaryWaitDataResponse() { data = waitDatas.ToList(),recordTotal = recordTotal,recordsFiltered = recordsFiltered};
             
        }
    }
}
