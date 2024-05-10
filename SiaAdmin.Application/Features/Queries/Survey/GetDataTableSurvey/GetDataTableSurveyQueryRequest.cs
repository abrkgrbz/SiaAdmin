using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.Survey.GetDataTableSurvey
{
    public class GetDataTableSurveyQueryRequest:IRequest<GetDataTableSurveyQueryResponse>
    {
        public string draw { get; set; }
        public int length { get; set; }
        public int start { get; set; }
        public string? orderColumnIndex { get; set; }
        public string? orderDir { get; set; }
        public string? orderColumnName { get; set; }
        public string? searchValue { get; set; }
    }
}
