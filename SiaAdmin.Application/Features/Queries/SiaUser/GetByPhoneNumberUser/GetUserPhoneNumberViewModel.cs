using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.SiaUser.GetByPhoneNumberUser
{
    public class GetUserPhoneNumberViewModel
    {
        public string UserGUID { get; set; }
        public string Guid { get; set; }
        public string PhoneNumber { get; set; }
    }
}
