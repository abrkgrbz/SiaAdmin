using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class SiaUserReadRepository:ReadRepository<SiaUser>,ISiaUserReadRepository
    {
        private readonly DbSet<SiaUser> _siaUsers;
       
        public SiaUserReadRepository(SiaAdminDbContext context) : base(context)
        {
            _siaUsers = context.Set<SiaUser>();
        }

        public bool IsUniqueUsername(string username)
        {
            return _siaUsers.All(x => x.UserName != username);
        }
    }
}
