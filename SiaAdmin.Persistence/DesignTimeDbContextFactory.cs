using Microsoft.EntityFrameworkCore;
using SiaAdmin.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;

namespace SiaAdmin.Persistence
{
    public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<SiaAdminDbContext>
    {
        public SiaAdminDbContext CreateDbContext(string[] args)
        {


            DbContextOptionsBuilder<SiaAdminDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseSqlServer(Configuration.ConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
