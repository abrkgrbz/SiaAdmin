using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Commands.Incentive.CreateIncentive
{
    public class CreateIncentiveHandler:IRequestHandler<CreateIncentiveRequest, CreateIncentiveResponse>
    {
        private IIncentiveWriteRepository _incentiveWriteRepository;

        public CreateIncentiveHandler(IIncentiveWriteRepository incentiveWriteRepository)
        {
            _incentiveWriteRepository = incentiveWriteRepository;
        }

        public Task<CreateIncentiveResponse> Handle(CreateIncentiveRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
