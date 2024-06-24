using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.User
{
    public class GetUserResponse
    {
        public UserViewModel User { get; set; }
        public string  RoleType { get; set; }
    }
}
