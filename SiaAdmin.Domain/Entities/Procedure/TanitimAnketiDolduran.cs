using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SiaAdmin.Domain.Entities.Procedure
{
    [Keyless]
    public class TanitimAnketiDolduran
    {
        public int Adet { get; set; }
        public int Durum { get; set; }
    }
}
