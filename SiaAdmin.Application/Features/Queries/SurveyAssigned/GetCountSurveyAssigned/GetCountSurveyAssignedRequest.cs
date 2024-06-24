using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.SurveyAssigned.GetCountSurveyAssigned
{
    public class GetCountSurveyAssignedRequest:IRequest<GetCountSurveyAssignedResponse>
    {
        public int Id { get; set; }
    }
}
