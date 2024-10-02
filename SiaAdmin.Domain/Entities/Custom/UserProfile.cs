using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SiaAdmin.Domain.Entities.Custom
{
    [Keyless]
    public class UserProfile
    {
        public string telephone { get; set; }
        public string? email { get; set; }
        public string? username { get; set; }
        public string? surname { get; set; }
        public int? totalmessaging { get; set; }
        public int? birthdate { get; set; }
        public int? gender { get; set; }
        public int? location { get; set; }
        public string? reference { get; set; }
        public string? referredby { get; set; }

    }
}
