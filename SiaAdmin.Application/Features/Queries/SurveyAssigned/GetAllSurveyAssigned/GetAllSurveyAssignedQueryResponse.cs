using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.SurveyAssigned.GetAllSurveyAssigned
{
    public class GetAllSurveyAssignedQueryResponse
    {
        public int recordTotal { get; set; }
        public int recordsFiltered { get; set; }
        public object SurveyAssignedList { get; set; }
    }
}
