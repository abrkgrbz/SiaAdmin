using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Interfaces.User;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly ISiaUserWriteRepository _siaUserWriteRepository;
        private readonly ISiaRoleReadRepository _userRoleReadRepository;
        private readonly ISiaUserReadRepository _siaUserReadRepository;
        private readonly ISiaUserRoleReadRepository _siaUserRoleReadRepository;

        public UserService(ISiaRoleReadRepository userRoleReadRepository, ISiaUserReadRepository siaUserReadRepository, ISiaUserRoleReadRepository siaUserRoleReadRepository, ISiaUserWriteRepository siaUserWriteRepository)
        {
            _userRoleReadRepository = userRoleReadRepository;
            _siaUserReadRepository = siaUserReadRepository;
            _siaUserRoleReadRepository = siaUserRoleReadRepository;
            _siaUserWriteRepository = siaUserWriteRepository;
        }

        public async Task<SiaUser> LoginUser(string username, string password)
        {
            var user = await _siaUserReadRepository.GetWhere(x => x.UserName.Equals(username) && x.Password.Equals(password))
                .FirstOrDefaultAsync();
            if (user == null)
                throw new ApiException("Kullanıcı adı veya şifre hatalı!");
            if (user.Approved == false)
                throw new ApiException("Kullanıcı Onayı gerekmekte!");
            return user;
        }

        public async Task<int> GetUserRole(int userId)
        {
            var role = await _siaUserRoleReadRepository.GetWhere(x => x.UserId.Equals(userId)).FirstOrDefaultAsync();
            if (role == null)
                throw new ApiException("Kullanıcı Rolu bulunamadı");
            return role.RoleId;
        }

        public async Task<string> GetRoleType(int roleId)
        {
            var roleType = await _userRoleReadRepository.GetWhere(x => x.Id.Equals(roleId)).FirstOrDefaultAsync();
            return roleType.RoleType;
        }

        public async Task<bool> CreateUser(SiaUser user)
        {
            var createdUser = await _siaUserWriteRepository.AddAsync(user);
            if (!createdUser)
                throw new ApiException("Beklenmedik bir hata");
            await _siaUserWriteRepository.SaveAsync();
            return true;
        }
    }
}
