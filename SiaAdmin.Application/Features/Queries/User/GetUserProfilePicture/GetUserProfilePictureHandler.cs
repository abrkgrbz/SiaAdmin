using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SiaAdmin.Application.Interfaces.SiaUser;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetUserProfilePicture
{
    public class GetUserProfilePictureHandler : IRequestHandler<GetUserProfilePictureRequest, Response<UserProfilePicture>>
    {
        readonly ISiaUserService _siaUserService;
        readonly IConfiguration configuration;

        public GetUserProfilePictureHandler(ISiaUserService siaUserService, IConfiguration configuration)
        {
            _siaUserService = siaUserService;
            this.configuration = configuration;
        }

        public async Task<Response<UserProfilePicture>> Handle(GetUserProfilePictureRequest request, CancellationToken cancellationToken)
        {
            var result = _siaUserService.GetUserProfilePicture(request.InternalGUID);
            return new Response<UserProfilePicture>(new UserProfilePicture() { picturePath = $"{configuration["BaseStorageUrl"]}{result}" });
        }
    }
}
