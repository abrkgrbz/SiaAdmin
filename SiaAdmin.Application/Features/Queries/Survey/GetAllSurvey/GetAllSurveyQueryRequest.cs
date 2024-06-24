using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.Survey.GetAllSurvey
{
    public class GetAllSurveyQueryRequest:IRequest<GetAllSurveyQueryResponse>
    {

        public int start { get; set; }
        public int length { get; set; }
    }
}
