using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.DTOs.SurveyAssigned
{
    public class MapSurveyAssigned
    {
        public string SurveyText { get; set; }
        public string SurveyLinkText { get; set; }
        public string SurveyLink { get; set; }
        public string SurveyDescription { get; set; }
        public DateTime Timestamp { get; set; }=DateTime.Now; 
        public int SurveyActive { get; set; } = 1;

         
    }
}
