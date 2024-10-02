using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.Survey.CloseSurvey
{
    public class CloseSurveyRequest:IRequest<Response<bool>>
    {
        public int SurveyId { get; set; }
    }
}
