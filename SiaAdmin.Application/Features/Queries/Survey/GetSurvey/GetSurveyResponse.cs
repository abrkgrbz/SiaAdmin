using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.DTOs.Survey;

namespace SiaAdmin.Application.Features.Queries.Survey.GetSurvey
{
    public class GetSurveyResponse
    {
        public bool success { get; set; }
        public List<SurveyDto> data { get; set; }
        public string message { get; set; }
    }

}
