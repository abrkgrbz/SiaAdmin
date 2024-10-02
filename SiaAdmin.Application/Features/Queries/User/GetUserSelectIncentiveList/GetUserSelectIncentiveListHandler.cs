using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetUserSelectIncentiveList
{
    public class GetUserSelectIncentiveListHandler:IRequestHandler<GetUserSelectIncentiveListRequest,Response<List<UserSelectIncentiveViewModel>>>
    {
        private IUserReadRepository _userReadRepository;
        private IMapper _mapper;
        public GetUserSelectIncentiveListHandler(IUserReadRepository userReadRepository, IMapper mapper)
        {
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }


        public async Task<Response<List<UserSelectIncentiveViewModel>>> Handle(GetUserSelectIncentiveListRequest request, CancellationToken cancellationToken)
        {
            var result = _userReadRepository.GetUserSelectIncentiveList(Guid.Parse(request.UserGUID));
            var mappingProfile = _mapper.Map<List<UserSelectIncentiveViewModel>>(result);
            return new Response<List<UserSelectIncentiveViewModel>>(mappingProfile);
        }
    }
}
