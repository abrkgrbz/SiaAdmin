using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.User.DeleteUser
{
    public class DeleteUserHandler:IRequestHandler<DeleteUserRequest,Response<int>>
    {
        private readonly IUserWriteRepository _userWriteRepository;

        public DeleteUserHandler(IUserWriteRepository userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
        }

        public async Task<Response<int>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _userWriteRepository.DeleteUser(request.InternalGUID);
            if (result==1)
            {
                return new Response<int>(result, "Kullanıcı başarıyla silindi");
            }

            return new Response<int>(result, "Silme işlemi başarısız");
             
        }
    }
}
