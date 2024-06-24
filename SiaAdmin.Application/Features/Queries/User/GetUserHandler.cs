using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Interfaces.User;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.User
{
    public class GetUserHandler : IRequestHandler<GetUserRequest, GetUserResponse>
    {
        private readonly IUserService _userService;
        private IMapper _mapper;
        public GetUserHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        public async Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userService.LoginUser(request.Username, request.Password);
            var roleId = await _userService.GetUserRole(user.Id);
            var roleType = await _userService.GetRoleType(roleId);
            var mapUser = _mapper.Map<UserViewModel>(user);
            return new GetUserResponse() { RoleType = roleType, User = mapUser };
        }
    }
}
