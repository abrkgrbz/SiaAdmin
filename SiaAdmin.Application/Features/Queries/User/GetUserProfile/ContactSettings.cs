using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.User.GetUserProfile
{
    public class ContactSettings
    {
        public bool IsCheckedSms { get; set; }
        public bool IsCheckedPhone { get; set; }
        public bool IsCheckedEmail { get; set; }
    }
}
