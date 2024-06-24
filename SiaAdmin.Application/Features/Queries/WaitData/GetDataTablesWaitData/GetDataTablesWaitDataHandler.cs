using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Features.Queries.Survey.GetDataTableSurvey;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.WaitData.GetDataTablesWaitData
{
    public class GetDataTablesWaitDataHandler:IRequestHandler<GetDataTablesWaitDataRequest,GetDataTablesWaitDataResponse>
    {
        private readonly IWaitDataReadRepository _waitDataReadRepository;

        public GetDataTablesWaitDataHandler(IWaitDataReadRepository waitDataReadRepository)
        {
            _waitDataReadRepository = waitDataReadRepository;
        }

        public async Task<GetDataTablesWaitDataResponse> Handle(GetDataTablesWaitDataRequest request, CancellationToken cancellationToken)
        {
            var waitData = _waitDataReadRepository.GetAll(false);
            int recordsFiltered = 0, recordTotal = 0;
            if (!string.IsNullOrEmpty(request.searchValue))
            {
                waitData = waitData.Where(x => x.SurveyUserGuid.ToString().ToLower().Contains(request.searchValue.ToLower())
                                               || x.SurveyId.ToString().Equals(request.searchValue));
            }

            if (!string.IsNullOrEmpty(request.orderColumnName) && !string.IsNullOrEmpty(request.orderDir))
            {
                waitData = await _waitDataReadRepository.OrderByField(waitData, request.orderColumnName, request.orderDir == "asc");
                recordsFiltered = waitData.Count();
                recordTotal = waitData.Count();

            }
            var waitDatas = await waitData.Skip(request.Start).Take(request.Length).ToListAsync();
            if (waitDatas == null) throw new Exception("Wait Data bulunamadı");
            return new GetDataTablesWaitDataResponse()
            {
                recordTotal = recordTotal,
                recordsFiltered = recordsFiltered,
                data = waitDatas
            };
        }
    }
}
