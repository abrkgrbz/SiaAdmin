using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.Point.GetPointListByInternalGuid
{
    public class GetPointListViewModel
    {
        public int SurveyPoints { get; set; }
        public DateTime TimeStamp { get; set; }
        public int SurveyId { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Approved { get; set; }
    }
}
