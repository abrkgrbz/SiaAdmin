using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.DTOs.Survey
{
    public class ListSurvey
    {
        public int Id { get; set; }
        public string SurveyText { get; set; }
        public string SurveyDescription { get; set; }
        public string SurveyLink { get; set; }
        public int? SurveyPoints { get; set; }
        public int? SurveyActive { get; set; }
    }
}
