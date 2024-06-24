using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.User
{
    public class GetUserRequest:IRequest<GetUserResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
