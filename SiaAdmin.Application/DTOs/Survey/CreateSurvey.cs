using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.DTOs.Survey
{
    public class CreateSurvey
    {
        [Required]
        public int SurveyId { get; set; }
        [Required]
        public string SurveyText { get; set; }
        [Required]
        public string SurveyDescription { get; set; }
        [Required]
        public string SurveyLink { get; set; }
        [Required]
        public string SurveyLinkText { get; set; }
        public DateTime? SurveyValidity { get; set; }
        private int SurveyActive { get; set; } = 1;
        public DateTime? SurveyStartDate { get; set; }
        [Required]
        public int SurveyPoints { get; set; }
        [Required]
        public int Mandatory { get; set; } 
        private DateTime Timestamp { get; set; } = DateTime.Now;
        [Required]
        public string DBAddress { get; set; }
        [Required]
        public string DPPass { get; set; }
        public string? DPUser { get; set; }
    }
}
