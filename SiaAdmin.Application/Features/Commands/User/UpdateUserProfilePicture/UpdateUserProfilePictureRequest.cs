using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.User.UpdateUserProfilePicture
{
    public class UpdateUserProfilePictureRequest:IRequest<Response<bool>>
    {
        public Guid InternalGUID { get; set; }
        public IFormFile Picture { get; set; }
    }
}
