using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Interfaces.User
{
    public interface IUserService
    {
        public Task<SiaUser> LoginUser(string username, string password);
        public Task<int> GetUserRole(int userId);
        public Task<string> GetRoleType(int roleId);

        public Task<bool> CreateUser(SiaUser user);
    }
}
