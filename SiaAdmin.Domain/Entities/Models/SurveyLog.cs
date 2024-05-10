using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public partial class SurveyLog : BaseEntity
    {
        public int SurveyId { get; set; }
        public Guid SurveyUserGuid { get; set; }

        public int SurveyPoints { get; set; }

        public int Active { get; set; }

        public DateTime TimeStamp { get; set; }

        public int Approved { get; set; }

        public string? Text { get; set; }

        public Guid? Extended { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
