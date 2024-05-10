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
    public class IncentiveReadRepository:ReadRepository<Incentive>,IIncentiveReadRepository
    {
        public IncentiveReadRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
