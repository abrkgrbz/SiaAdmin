using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public partial class UserLog : BaseEntity
    {

        public int UserId { get; set; }

        public string Description { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
    }
}
