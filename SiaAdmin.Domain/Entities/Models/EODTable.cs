using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public partial class EODTable : BaseEntity
    { 
        public Guid SurveyUserGuid { get; set; }

        public int? ToplamKatilim { get; set; }

        public int? OlumluKatilim { get; set; }

        public int? DavetEdilenAnketSayisi { get; set; }

        public int? DavetEdilenArkadasSayisi { get; set; }

        public decimal? ToplamPuan { get; set; }

        public decimal? ToplamPara { get; set; }

        public DateTime? Timestamp { get; set; }
    }
}
