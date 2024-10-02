using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.User.UpdateUserProfile
{
    public class UpdateUserProfilHandler:IRequestHandler<UpdateUserProfileRequest,Response<int>>
    {
        private IUserWriteRepository _userWriteRepository;

        public UpdateUserProfilHandler(IUserWriteRepository userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
        }

        public async Task<Response<int>> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            var result = await _userWriteRepository.UpdateUserProfile(request.Email,request.Name,request.Surname,request.Birthdate,7,request.Sex,request.Location,request.InternalGuid);
            return new Response<int>(1, "Kullanıcı güncelleme işlemi başarılı");
        }
    }
}
