using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Commands.SurveyLog.CreateIncentive
{
    public class CreateIncentiveRequest:IRequest<CreateIncentiveResponse>
    {
        public int IncentiveId { get; set; }
        public Guid InternalGuid { get; set; }
    }
}
