using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Custom;

namespace SiaAdmin.Application.Features.Queries.User.GetAllChurnDataList
{
    public class GetAllChurnDataListHandler:IRequestHandler<GetAllChurnDataListRequest,GetAllChurnDataListResponse>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IMapper _mapper;
        public GetAllChurnDataListHandler(IUserReadRepository userReadRepository, IMapper mapper)
        {
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }

        public async Task<GetAllChurnDataListResponse> Handle(GetAllChurnDataListRequest request, CancellationToken cancellationToken)
        {
            var data = await _userReadRepository.GetAllChurnData();
            var mapping = _mapper.Map<List<ChurnData>, List<ChurnDataViewModel>>(data);
            return new GetAllChurnDataListResponse() { ChurnDataViewModels = mapping };
        }
    }
}
