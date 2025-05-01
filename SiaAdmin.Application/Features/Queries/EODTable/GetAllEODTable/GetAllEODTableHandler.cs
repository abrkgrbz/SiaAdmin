using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.EODTable.GetAllEODTable
{
	public class GetAllEODTableHandler:IRequestHandler<GetAllEODTableRequest,GetAllEODTableResponse>
	{
		private IEODTableReadRepository _eodTableReadRepository;
		private IMapper _mapper;
		public GetAllEODTableHandler(IEODTableReadRepository eodTableReadRepository, IMapper mapper)
        {
            _eodTableReadRepository = eodTableReadRepository;
            _mapper = mapper;
        }


        public async Task<GetAllEODTableResponse> Handle(GetAllEODTableRequest request, CancellationToken cancellationToken)
        {
            var result = _eodTableReadRepository.GetWhere(x => x.ToplamPara > 0, false).OrderByDescending(x => x.ToplamPara).ToList();
            var mapping = _mapper.Map<List<Domain.Entities.Models.EODTable>, List<EODTableViewModel>>(result);
            return new GetAllEODTableResponse() { EodTableViewModels = mapping };
        }
    }
}
