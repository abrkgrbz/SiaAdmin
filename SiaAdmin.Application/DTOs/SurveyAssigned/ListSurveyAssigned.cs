using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.DTOs.SurveyAssigned
{
    public class ListSurveyAssigned
    {
        public int Id { get; set; }
        public int? SurveyId { get; set; }
        public int SurveyPoints { get; set; }

        public Guid InternalGuid { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? SurveyValidity { get; set; }

        private int SurveyActive { get; set; }

        public DateTime? SurveyStartDate { get; set; }
    }
}
