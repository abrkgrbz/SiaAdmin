using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.UserLog
{
    public class UserLogReadRepository:ReadRepository<Domain.Entities.Models.UserLog>,IUserLogReadRepository
    {
        public UserLogReadRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
