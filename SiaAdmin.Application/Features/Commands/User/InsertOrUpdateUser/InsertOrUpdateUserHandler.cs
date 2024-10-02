using MediatR;
using SiaAdmin.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Commands.User.InsertOrUpdateUser
{
    public class InsertOrUpdateUserHandler:IRequestHandler<InsertOrUpdateUserRequest,InsertOrUpdateUserResponse>
    {
        private IUserWriteRepository _userWriteRepository;
        private IUserReadRepository _userReadRepository;

        public InsertOrUpdateUserHandler(IUserWriteRepository userWriteRepository, IUserReadRepository userReadRepository)
        {
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
        }

        public async Task<InsertOrUpdateUserResponse> Handle(InsertOrUpdateUserRequest request, CancellationToken cancellationToken)
        {
            bool checkUser = _userReadRepository.IfExistUser(request.Phone);
            _userWriteRepository.InsertOrUpdateUser(request.RegionCode, request.Phone, request.Ip, request.Browser, checkUser);
            return new InsertOrUpdateUserResponse() { IsSucceded = true };

        }
    }
}
