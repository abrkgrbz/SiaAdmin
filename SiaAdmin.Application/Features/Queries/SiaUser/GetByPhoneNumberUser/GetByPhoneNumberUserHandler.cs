using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.SiaUser.GetByPhoneNumberUser
{
    public class GetByPhoneNumberUserHandler:IRequestHandler<GetByPhoneNumberUserRequest,GetByPhoneNumberUserResponse>
    {
        private IUserReadRepository _userReadRepository;
        private IMapper _mapper;
        public GetByPhoneNumberUserHandler(IUserReadRepository userReadRepository, IMapper mapper)
        {
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }

        public async Task<GetByPhoneNumberUserResponse> Handle(GetByPhoneNumberUserRequest request, CancellationToken cancellationToken)
        {
            var user =await _userReadRepository.GetWhere(x => x.Msisdn == request.PhoneNumber).OrderBy(x=>x.RegistrationDate).LastOrDefaultAsync();
            var mapUser = _mapper.Map<GetUserPhoneNumberViewModel>(user);
            if (user == null)
                throw new ApiException("Kullanıcı bulunamadı");
            return new GetByPhoneNumberUserResponse() { GetUserPhoneNumberViewModel = mapUser};
            
        }
    }
}
