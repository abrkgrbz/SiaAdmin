using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.Point.GetPointListBySurveyId
{
    public class GetPointListBySurveyIdRequest:IRequest<GetPointListBySurveyIdResponse>
    {
        public int SurveyId { get; set; }
    }
}
