using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class SiaUserRoleReadRepository:ReadRepository<Domain.Entities.Models.SiaUserRole>,ISiaUserRoleReadRepository
    {
        public SiaUserRoleReadRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
