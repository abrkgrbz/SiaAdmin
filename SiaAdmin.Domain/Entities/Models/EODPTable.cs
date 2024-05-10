using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public partial class EODPTable : BaseEntity
    {
        public int? SurveyId { get; set; }
        public string? SurveyText { get; set; }

        public string? SurveyDescription { get; set; }

        public DateTime? SurveyValidity { get; set; }

        public DateTime? SurveyStartDate { get; set; }

        public string? SurveyDbaddress { get; set; }

        public string? SurveyStatus { get; set; }

        public int? SurveyCompleteCount { get; set; }

        public int? SurveyAssigned { get; set; }
        public int? SurveyAssignedAvail { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
