using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public partial class SiaRole : BaseEntity
    {
        public string RoleType { get; set; }
        public virtual ICollection<SiaUserRole> SiaUserRoles { get; set; } = new List<SiaUserRole>();
    }
}
