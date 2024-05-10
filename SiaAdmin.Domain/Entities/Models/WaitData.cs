using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Domain.Entities.Models
{
    public partial class WaitData : BaseEntity
    {

        public int SurveyId { get; set; }

        public Guid SurveyUserGuid { get; set; }

        public DateTime Tarih { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
