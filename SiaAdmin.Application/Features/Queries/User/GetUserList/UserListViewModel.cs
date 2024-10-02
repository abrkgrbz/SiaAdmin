using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.User.GetUserList
{
    public class UserListViewModel
    {
        public int Id { get; set; } 
        public string Fullname { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastFailedLoginDate { get; set; }
        public bool Approved { get; set; }
    }
}
