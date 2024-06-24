using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Features.Queries.EODTable;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;
using SiaAdmin.Domain.Entities.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SiaAdmin.Application.Features.Queries.EODPTable
{
    public class EODPTableQueryHandler:IRequestHandler<EODPTableQueryRequest, EODPTableQueryResponse>
    {
        private readonly IEODPTableReadRepository _eodpTableReadRepository;
        private IMapper _mapper;
        public EODPTableQueryHandler(IEODPTableReadRepository eodpTableReadRepository, IMapper mapper)
        {
            _eodpTableReadRepository = eodpTableReadRepository;
            _mapper = mapper;
        }

        public async Task<EODPTableQueryResponse> Handle(EODPTableQueryRequest request, CancellationToken cancellationToken)
        {
            var eodpTable = _eodpTableReadRepository.GetAll(false);
            int recordsFiltered = 0, recordTotal = 0;
            if (!string.IsNullOrEmpty(request.searchValue))
            {
                eodpTable = eodpTable.Where(x => x.SurveyText.ToLower().Contains(request.searchValue.ToLower())
                                                 || x.SurveyDescription.ToLower().Contains(request.searchValue.ToLower())
                                                 || x.SurveyId.Equals(Convert.ToInt32(request.searchValue)));
            }

            if (!string.IsNullOrEmpty(request.orderColumnName) && !string.IsNullOrEmpty(request.orderDir))
            {
                eodpTable = await _eodpTableReadRepository.OrderByField(eodpTable, request.orderColumnName, request.orderDir == "asc");
                recordsFiltered = eodpTable.Count();
                recordTotal = eodpTable.Count();
            }

            var reponseData = await eodpTable.Skip(request.start).Take(request.length).ToListAsync();

            return new()
            {
                recordTotal = recordTotal,
                data = reponseData,
                recordsFiltered = recordsFiltered
            };
        }
    }
}
