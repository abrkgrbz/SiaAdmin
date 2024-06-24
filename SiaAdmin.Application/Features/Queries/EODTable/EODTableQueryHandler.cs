using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Features.Queries.EODPTable;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SiaAdmin.Application.Features.Queries.EODTable
{
    public class EODTableQueryHandler:IRequestHandler<EODTableQueryRequest, EODTableQueryResponse>
    {
        private readonly IEODTableReadRepository _eodTableReadRepository; 
        private IMapper _mapper;
        public EODTableQueryHandler(IEODTableReadRepository eodTableReadRepository, IMapper mapper)
        {
            _eodTableReadRepository = eodTableReadRepository;
            _mapper = mapper;
        }


        public async Task<EODTableQueryResponse> Handle(EODTableQueryRequest request, CancellationToken cancellationToken)
        {
            
            var eodTable = _eodTableReadRepository.GetAll(false);
            int recordsFiltered = 0, recordTotal = 0;
            if (!string.IsNullOrEmpty(request.searchValue))
            {
                eodTable = eodTable.Where(x => x.SurveyUserGuid.ToString().Contains(request.searchValue.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.orderColumnName) && !string.IsNullOrEmpty(request.orderDir))
            {
                eodTable = await _eodTableReadRepository.OrderByField(eodTable, request.orderColumnName, request.orderDir == "asc");
                recordsFiltered = eodTable.Count();
                recordTotal = eodTable.Count();
            }

            var reponseData = await eodTable.Skip(request.Start).Take(request.Length).ToListAsync();
            return new EODTableQueryResponse()
            {
                recordTotal = recordTotal,
                data = reponseData,
                recordsFiltered = recordsFiltered
            };
        }
    }
}
