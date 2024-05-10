using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public class Scm : BaseEntity
    {
        public int Scmid { get; set; }

        public string? Scm1 { get; set; }

        public int Active { get; set; }
    }
}
