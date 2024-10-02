using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Repositories
{
    public interface IUserWriteRepository:IWriteRepository<User>
    {
        public void InsertOrUpdateUser(string regionCode, string msisdn, string ip, string browser, bool checkUser);
        Task<int> DeleteUser(Guid internalGuid);

        Task<int> UpdateUserProfile(string email,string name,string surname,int birthdate,int contactChannel,int sex,int location,Guid internalGuid);

   
    }
}
