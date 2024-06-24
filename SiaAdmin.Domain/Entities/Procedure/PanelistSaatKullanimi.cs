 
using Microsoft.EntityFrameworkCore;

namespace SiaAdmin.Domain.Entities.Procedure
{
    [Keyless]
    public class PanelistSaatKullanimi
    {
        public int Saat { get; set; }
        public int? Pazartesi { get; set; }
        public int? Sali { get; set; }
        public int? Carsamba { get; set; }
        public int? Persembe { get; set; }
        public int? Cuma { get; set; }
        public int? Cumartesi { get; set; }
        public int? Pazar { get; set; }
    }
}
