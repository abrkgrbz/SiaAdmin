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
    public class BlockListReadRepository:ReadRepository<BlockList>,IBlockListReadRepository
    {
        public BlockListReadRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
