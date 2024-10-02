using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetUserProfilePicture
{
    public class GetUserProfilePictureRequest:IRequest<Response<UserProfilePicture>>
    {
        public Guid InternalGUID { get; set; }
    }
}
