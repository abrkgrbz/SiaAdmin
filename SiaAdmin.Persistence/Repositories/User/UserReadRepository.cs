using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class UserReadRepository:ReadRepository<Domain.Entities.Models.User>,IUserReadRepository
    {
        private readonly DbSet<User> _users;
        public UserReadRepository(SiaAdminDbContext context) : base(context)
        {
            _users = context.Set<User>();
        }

        public List<Guid> ConvertInternalGuid(List<Guid> userGuids)
        {
            
            List<Guid> convertedInternalGuids= new List<Guid>();
            try
            {
                foreach (var item in userGuids)
                {
                    _users.FirstOrDefaultAsync(x => x.SurveyUserGuid == item);
                    convertedInternalGuids.Add(item);
                }
            }
            catch (Exception e)
            {
                throw new ApiException("Hatalı GUID'lar mevcut");
            }

            return convertedInternalGuids;
        }
    }
}
