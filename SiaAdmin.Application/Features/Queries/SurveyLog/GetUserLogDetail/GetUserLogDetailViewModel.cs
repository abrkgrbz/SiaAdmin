using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.SurveyLog.GetUserLogDetail
{
    public class GetUserLogDetailViewModel
    {
        public int SurveyId { get; set; }
        public int SurveyPoints { get; set; }
        public int Active { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Text { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Approved { get; set; }
    }
}
