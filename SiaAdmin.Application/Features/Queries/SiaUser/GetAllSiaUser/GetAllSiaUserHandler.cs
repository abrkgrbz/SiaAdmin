using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Features.Queries.EODTable;
using SiaAdmin.Application.Features.Queries.Survey.GetDataTableSurvey;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Features.Queries.SiaUser.GetAllSiaUser
{
    public class GetAllSiaUserHandler : IRequestHandler<GetAllSiaUserRequest, GetAllSiaUserResponse>
    {
        private readonly IUserReadRepository _userReadRepository;
        private IMapper _mapper;
        public GetAllSiaUserHandler(IUserReadRepository userReadRepository, IMapper mapper)
        {
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }

        public async Task<GetAllSiaUserResponse> Handle(GetAllSiaUserRequest request, CancellationToken cancellationToken)
        {
            request.orderColumnName = "SurveyUserGuid";
            var siaUserList = _userReadRepository.GetWhere(x=>x.Active==1,false) ; 
            int recordsFiltered = 0, recordTotal = 0;
            if (!string.IsNullOrEmpty(request.searchValue))
            {
                siaUserList = siaUserList.Where(x =>
                    x.SurveyUserGuid.ToString().ToLower().Contains(request.searchValue.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.orderColumnName) && !string.IsNullOrEmpty(request.orderDir))
            {
                siaUserList = await _userReadRepository.OrderByField(siaUserList, request.orderColumnName, request.orderDir == "asc");
                recordsFiltered = siaUserList.Count();
                recordTotal = siaUserList.Count();
            }


            var reponseData = await siaUserList.Skip(request.Start).Take(request.Length).ToListAsync();
            return new()
            {
                recordTotal = recordTotal,
                recordsFiltered = recordsFiltered,
                data = reponseData
            };
        }
    }
}
