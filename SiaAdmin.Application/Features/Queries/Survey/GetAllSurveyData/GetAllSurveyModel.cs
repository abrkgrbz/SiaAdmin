using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.Survey.GetAllSurveyData
{
    public  class GetAllSurveyModel
    {
        public int Id { get; set; }
        public string SurveyText { get; set; }
        public string SurveyLink { get; set; }
        public string SurveyLinkText { get; set; }
        public string SurveyDescription { get; set; }
        public DateTime? SurveyValidity { get; set; }
        public int SurveyActive { get; set; }
        public DateTime? SurveyStartDate { get; set; }
        public int SurveyPoints { get; set; }
        public int Mandotory { get; set; }
        public DateTime Timestamp { get; set; }
        public string DBAdress { get; set; }
    }
}
