using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Interfaces.SiaUser;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.User.UpdateUserProfilePicture
{
    public class UpdateUserProfilePictureHandler : IRequestHandler<UpdateUserProfilePictureRequest, Response<bool>>
    {
        private ISiaUserService _siaUserService;

        public UpdateUserProfilePictureHandler(ISiaUserService siaUserService)
        {
            _siaUserService = siaUserService;
        }

        public async Task<Response<bool>> Handle(UpdateUserProfilePictureRequest request, CancellationToken cancellationToken)
        {
            var result = await _siaUserService.UploadProfilePicture(request.Picture, request.InternalGUID);
            if (result.Length > 0)
            {
                return new Response<bool>(true, "Profil resminiz yüklendi");
            }
            throw new ApiException("Profil resmi yüklenirken beklenmedik hata");
        }
    }
}
