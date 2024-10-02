using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Commands.SurveyLog.CreateIncentive
{
    public class CreateIncentiveHandler:IRequestHandler<CreateIncentiveRequest,CreateIncentiveResponse>
    {
        public Task<CreateIncentiveResponse> Handle(CreateIncentiveRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
