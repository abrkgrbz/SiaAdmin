using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.User.GetUserProfile
{
    public class GetUserProfileViewModel
    {
        public string telephone { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string surname { get; set; }
        public ContactSettings ContactSettings { get; set; }
        public int birthdate { get; set; }
        public int gender { get; set; }
        public int location { get; set; }
        public string reference { get; set; }
        public string referredby { get; set; }
    }
}
