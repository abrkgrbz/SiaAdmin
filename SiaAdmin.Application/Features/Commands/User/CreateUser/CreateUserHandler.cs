using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Interfaces;
using SiaAdmin.Application.Interfaces.User;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Features.Commands.User.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
    {
        private IMapper _mapper;
        private readonly IUserService _userService;
        public CreateUserHandler(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {

            try
            {

                var mapUser = _mapper.Map<SiaUser>(request);
                mapUser.UserGUID=Guid.NewGuid();
                var response = await _userService.CreateUser(mapUser);
                return new CreateUserResponse()
                {
                    Message = "Kayıt İşlemi Başarıyla Gerçekleştirildi! Lütfen Admin Onayı Bekleyiniz!",
                    Succeeded = true
                };
            }
            catch (Exception e)
            {
                return new CreateUserResponse() { Message = e.Message, Succeeded = false };
            }

        }
    }
}
