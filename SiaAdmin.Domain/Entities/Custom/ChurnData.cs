using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Domain.Entities.Custom
{
    [Keyless]
    public class ChurnData
    {
        public Guid InternalGUID { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? Msisdn { get; set; }
        public string? Email { get; set; }
        public int cazipdegil { get; set; }
        public int ilgisiz { get; set; }
        public int kimole { get; set; }
        public int korkuyorum { get; set; }
        public int vakitsizim { get; set; }
        public int dusukcene { get; set; }
        public int diger { get; set; }
    }
}
