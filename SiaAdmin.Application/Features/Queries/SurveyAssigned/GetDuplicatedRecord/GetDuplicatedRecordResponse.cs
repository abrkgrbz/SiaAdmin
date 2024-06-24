using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.SurveyAssigned.GetDuplicatedRecord
{
    public class GetDuplicatedRecordResponse
    {
        public List<DuplicatedRecordViewModel> DuplicatedRecordViewModels { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
    }
}
