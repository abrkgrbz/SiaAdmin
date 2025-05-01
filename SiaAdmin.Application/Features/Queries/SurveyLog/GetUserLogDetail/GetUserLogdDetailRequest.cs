using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.SurveyLog.GetUserLogDetail
{
    public class GetUserLogdDetailRequest:IRequest<List<GetUserLogDetailViewModel>>
    {
        public string SurveyUserGUID { get; set; }
        public int draw { get; set; } 
        public int Length { get; set; }
        public int Start { get; set; } 
        public string? orderColumnIndex { get; set; }
        public string? orderDir { get; set; }
        public string? orderColumnName { get; set; }
        public string? searchValue { get; set; }
    }
}
