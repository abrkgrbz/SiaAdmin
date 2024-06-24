using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.User
{
    public class UserViewModel
    {
        public Guid UserGUID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; }  
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastFailedLoginDate { get; set; } 
    }
}
