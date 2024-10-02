using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.SiaUser.GetByGuidSiaUser
{
    public class GetUserByGuidViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public int Active { get; set; }
        public int LoginCount { get; set; }
        public string Msisdn { get; set; }
        public string Email { get; set; }
        public string LastIP { get; set; }
        public int ProfilPuani { get; set; }
    }
}
