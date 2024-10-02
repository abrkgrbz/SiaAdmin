using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.SurveyLog.CreateSurveyLogByUser
{
    public class CreateSurveyLogByUserRequest:IRequest<Response<int>>
    {
        public int IncentiveId { get; set; }
        public Guid InternalGuid { get; set; }
    }
}
