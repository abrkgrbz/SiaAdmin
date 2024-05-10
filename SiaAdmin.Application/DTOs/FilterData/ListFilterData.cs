using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.DTOs.FilterData
{
    public class ListFilterData
    {
        public string SurveyUserGUID { get; set; }
        public int? ACIKYAS { get; set; }
        public int? GRUPYAS { get; set; }
        public int? CINSIYET { get; set; }
        public int? IL { get; set; }
        public int? ILCE { get; set; }
        public int? BOLGE { get; set; }
        public int? HHR { get; set; }
        public int? EgitimGK { get; set; }
        public int? EgitimHR { get; set; }
        public int? Emekli { get; set; }
        public string? AMeslekGK { get; set; }
        public string? AMeslekHR { get; set; }
        public int? YMeslekGK { get; set; }
        public int? YMeslekHR { get; set; }
        public int? YMeslekDetayGK { get; set; }
        public int? YMeslekDetayHR { get; set; }
        public int? YSES { get; set; }
        public int? G02 { get; set; }
        public int? G03 { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
