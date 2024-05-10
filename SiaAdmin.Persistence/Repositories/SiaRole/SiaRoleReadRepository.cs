using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class SiaRoleReadRepository:ReadRepository<SiaRole>,ISiaRoleReadRepository
    {
        public SiaRoleReadRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
