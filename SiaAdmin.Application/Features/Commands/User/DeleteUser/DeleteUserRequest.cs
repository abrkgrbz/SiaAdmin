using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.User.DeleteUser
{
    public class DeleteUserRequest:IRequest<Response<int>>
    {
        public Guid InternalGUID { get; set; }
    }
}
