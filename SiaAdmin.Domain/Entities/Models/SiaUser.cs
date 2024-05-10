using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public partial class SiaUser : BaseEntity
    {
        public Guid UserGUID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastFailedLoginDate { get; set; }
        public bool Approved { get; set; }
        public virtual ICollection<SiaUserRole> SiaUserRoles { get; set; } = new List<SiaUserRole>();
    }
}
