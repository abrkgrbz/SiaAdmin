using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetUserProfile
{
    public class GetUserProfileRequest:IRequest<Response<GetUserProfileViewModel>>
    {
        public string UserGuid { get; set; }

    }
}
