using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.SurveyAssigned.GetUserGuidBySurveyAssigned
{
    public class GetUserGuidBySurveyAssignedRequest:IRequest<GetUserGuidBySurveyAssignedResponse>
    {
        public int Id { get; set; }
    }
}
