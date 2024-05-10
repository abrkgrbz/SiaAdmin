using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.DTOs.FilterData;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SiaAdmin.Application.Features.Queries.FilterData.GetDataTableFilterData
{
    public class GetDataTableFilterDataQueryHandler:IRequestHandler<GetDataTableFilterDataQueryRequest,GetDataTableFilterDataQueryResponse>
    {
        private IFilterDataReadRepository _filterDataReadRepository;
        private IMapper _mapper;
        public GetDataTableFilterDataQueryHandler(IFilterDataReadRepository filterDataReadRepository, IMapper mapper)
        {
            _filterDataReadRepository = filterDataReadRepository;
            _mapper = mapper;
        }

        public async Task<GetDataTableFilterDataQueryResponse> Handle(GetDataTableFilterDataQueryRequest request, CancellationToken cancellationToken)
        {
            var list = _filterDataReadRepository.GetAll(false);
            int recordsFiltered = 0, recordTotal = 0;
            if (!string.IsNullOrEmpty(request.searchValue))
            {
                list = list.Where(x => x.SurveyUserGuid.ToLower().Contains(request.searchValue.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.orderColumnName) && !string.IsNullOrEmpty(request.orderDir))
            {
                list = await _filterDataReadRepository.OrderByField(list, request.orderColumnName, request.orderDir == "asc");
                recordsFiltered = list.Count();
                recordTotal = list.Count();
            }

            var newList = await list.Skip(request.start).Take(request.length).ToListAsync();
            if (newList == null) throw new ApiException("Liste bulunamadı");
            return new GetDataTableFilterDataQueryResponse()
            {
                draw = request.draw,
                recordTotal = recordTotal,
                recordsFiltered = recordsFiltered,
                FilterDatas = newList
            };
        }
    }
}
