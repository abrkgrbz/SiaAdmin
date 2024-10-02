using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Features.Commands.UserLogs.CreateUserLogs
{
    public class CreateUserLogsHandler:IRequestHandler<CreateUserLogsRequest,Response<int>>
    {
        private IUserLogWriteRepository _userLogWriteRepository;

        public CreateUserLogsHandler(IUserLogWriteRepository userLogWriteRepository)
        {
            _userLogWriteRepository = userLogWriteRepository;
        }

        public async Task<Response<int>> Handle(CreateUserLogsRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
