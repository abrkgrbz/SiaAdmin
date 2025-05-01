using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.Incentive.GetAllIncentice
{
    public class GetAllIncenticeHandler:IRequestHandler<GetAllIncenticeRequest, Response<List<IncentiveViewModel>>>
    {
        private IIncentiveReadRepository _incentiveReadRepository;
        private IMapper _mapper;

        public GetAllIncenticeHandler(IIncentiveReadRepository incentiveReadRepository, IMapper mapper)
        {
            _incentiveReadRepository = incentiveReadRepository;
            _mapper = mapper;
        }


        public async Task<Response<List<IncentiveViewModel>>> Handle(GetAllIncenticeRequest request, CancellationToken cancellationToken)
        {
            var incentiveList =await _incentiveReadRepository.GetWhere(x=>x.ShowInDisplay==1 && x.Active==1).ToListAsync();
            var mappingProfile = _mapper.Map<List<IncentiveViewModel>>(incentiveList);
            return new Response<List<IncentiveViewModel>>(mappingProfile);
          
        }
    }
}
