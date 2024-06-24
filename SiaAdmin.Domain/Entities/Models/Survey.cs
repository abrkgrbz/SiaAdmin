using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public class Survey : BaseEntity
    {
    
        public string SurveyText { get; set; }
        public string SurveyDescription { get; set; }
        public string SurveyLink { get; set; }
        public string SurveyLinkText { get; set; }
        public DateTime? SurveyValidity { get; set; }
        public int? SurveyActive { get; set; }
        public DateTime? SurveyStartDate { get; set; }
        public string? SurveyRedirect { get; set; }
        public int SurveyPoints { get; set; }
        public int? Mandatory { get; set; }
        public DateTime Timestamp { get; set; }=DateTime.Now;
        public string? DBAddress { get; set; }
        public string? DPPass { get; set; }
        public string? DPUser { get; set; }
    }
}
