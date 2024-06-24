using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Commands.User.CreateUser
{
    public class CreateUserRequest:IRequest<CreateUserResponse>
    {
 
        public string Password { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool Approved { get; set; }=false;
        public DateTime CreatedDate { get; set; }=DateTime.Now;
    }
}
