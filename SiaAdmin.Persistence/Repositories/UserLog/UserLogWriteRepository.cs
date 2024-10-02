using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.UserLog
{
    public class UserLogWriteRepository:WriteRepository<Domain.Entities.Models.UserLog>,IUserLogWriteRepository
    {
        public UserLogWriteRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
