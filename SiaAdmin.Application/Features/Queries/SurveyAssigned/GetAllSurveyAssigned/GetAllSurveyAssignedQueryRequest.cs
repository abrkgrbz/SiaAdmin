using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.SurveyAssigned.GetAllSurveyAssigned
{
    public class GetAllSurveyAssignedQueryRequest:IRequest<GetAllSurveyAssignedQueryResponse>
    {
        public int length { get; set; }
        public int start { get; set; }

    }
}
