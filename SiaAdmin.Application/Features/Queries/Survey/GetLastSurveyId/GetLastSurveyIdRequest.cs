using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.Survey.GetLastSurveyId
{
    public class GetLastSurveyIdRequest:IRequest<GetLastSurveyIdResponse>
    {
    }
}
