using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.Point.GetPointListBySurveyId
{
    public class GetPointListBySurveyIdViewModel
    {
        public Guid SurveyUserGUID { get; set; }
        public int SurveyPoints { get; set; }
        public int Active { get; set; }
        public int Approved { get; set; }
        public string? Text { get; set; }
        public string? Extended { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
