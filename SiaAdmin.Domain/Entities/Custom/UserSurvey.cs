using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SiaAdmin.Domain.Entities.Custom
{
    [Keyless]
    public class UserSurvey
    {
        public Guid SurveyUserGUID { get; set; }
        public int SurveyId { get; set; }
        public string SurveyText { get; set; }
        public string SurveyDescription { get; set; }
        public string SurveyLink { get; set; }
        public string SurveyLinkText { get; set; }
        public DateTime? SurveyValidity { get; set; }
        public int? SurveyActive { get; set; }
        public DateTime? SurveyStartDate { get; set; }
        public string? SurveyRedirect { get; set; }
        public int SurveyPoints { get; set; }
        public int? Mandatory { get; set; }
        public DateTime TimeStamp { get; set; }
     
    }
}
