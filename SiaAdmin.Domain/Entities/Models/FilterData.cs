using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public class FilterData : BaseEntity
    {

        public string? SurveyUserGuid { get; set; }

        public int? Acikyas { get; set; }

        public int? Grupyas { get; set; }

        public int? Cinsiyet { get; set; }

        public int? Il { get; set; }

        public int? Ilce { get; set; }

        public int? Bolge { get; set; }

        public int? Hhr { get; set; }

        public int? EgitimGk { get; set; }

        public int? EgitimHr { get; set; }

        public int? Emekli { get; set; }

        public string? AmeslekGk { get; set; }

        public string? AmeslekHr { get; set; }

        public int? YmeslekGk { get; set; }

        public int? YmeslekHr { get; set; }

        public int? YmeslekDetayGk { get; set; }

        public int? YmeslekDetayHr { get; set; }

        public int? Yses { get; set; }

        public int? G02 { get; set; }

        public int? G03 { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
