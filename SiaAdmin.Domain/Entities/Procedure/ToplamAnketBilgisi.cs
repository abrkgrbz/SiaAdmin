using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SiaAdmin.Domain.Entities.Procedure
{
    [Keyless]
    public class ToplamAnketBilgisi
    {
        public int Adet { get; set; }
        public int Kampanya { get; set; }
    }
}
