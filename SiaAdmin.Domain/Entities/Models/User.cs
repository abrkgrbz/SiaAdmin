using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public partial class User : BaseEntity
    { 
        public Guid InternalGuid { get; set; }

        public Guid SurveyUserGuid { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int Active { get; set; }

        public DateTime? LastLogin { get; set; }

        public DateTime? LastFailedLogin { get; set; }

        public int LoginCount { get; set; }

        public string? RegionCode { get; set; }

        public string? Msisdn { get; set; }

        public string? Email { get; set; }

        public string? TCKNo { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public int ContactChannel { get; set; }

        public int? Birthdate { get; set; }

        public int? Sex { get; set; }

        public int? Location { get; set; }

        public string? LastIp { get; set; }

        public string? LastBrowser { get; set; }

        public string? ReferredBy { get; set; }

        public string? MyReference { get; set; }

        public int InternalUser { get; set; }

        public int Kontrolde { get; set; }

        public int ProfilPuani { get; set; }

        public string? SCMTarihi { get; set; }

        public DateTime? IYS { get; set; }
    }
}
