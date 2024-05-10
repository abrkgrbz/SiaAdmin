using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.DTOs.SurveyAssigned
{
    public class MapCreateSurveyAssigned
    {
        public int SurveyId { get; set; }
        public DateTime? SurveyValidity { get; set; }
        public DateTime? SurveyStartDate { get; set; }
        public int SurveyPoints { get; set; }
        public Guid InternalGuid { get; set; }
        
    }
}
