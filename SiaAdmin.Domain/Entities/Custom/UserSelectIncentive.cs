using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Domain.Entities.Custom
{
    [Keyless]
    public class UserSelectIncentive
    {
        public int Id { get; set; }
        public string IncentiveText { get; set; }
        public int Points { get; set; }
    }
}
