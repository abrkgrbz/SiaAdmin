using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetUserRecievedGiftsList
{
    public class GetUserRecievedGiftsListHandler : IRequestHandler<GetUserRecievedGiftsListRequest, Response<List<UserRecievedGiftViewModel>>>
    {
        private IUserReadRepository _userReadRepository;
        private IMapper _mapper;

        public GetUserRecievedGiftsListHandler(IUserReadRepository userReadRepository, IMapper mapper)
        {
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }


        public async Task<Response<List<UserRecievedGiftViewModel>>> Handle(GetUserRecievedGiftsListRequest request, CancellationToken cancellationToken)
        {
            var result = _userReadRepository.GetUserRecievedGiftsList(Guid.Parse(request.UserGUID));
            var mappingProfile = _mapper.Map<List<UserRecievedGiftViewModel>>(result);
            return new Response<List<UserRecievedGiftViewModel>>(mappingProfile);
        }
    }
}
