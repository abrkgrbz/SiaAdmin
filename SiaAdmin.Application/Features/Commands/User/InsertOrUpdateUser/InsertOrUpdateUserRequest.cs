using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Commands.User.InsertOrUpdateUser
{
    public class InsertOrUpdateUserRequest:IRequest<InsertOrUpdateUserResponse>
    {
        public string RegionCode { get; set; }
        public string Phone { get; set; }
        public string Browser { get; set; } 
        public string Ip { get; set; } 
    }
}
