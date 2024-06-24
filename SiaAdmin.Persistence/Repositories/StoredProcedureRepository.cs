using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories
{
    public class StoredProcedureRepository<T> : IStoredProcedureRepository<T> where T : class,new()
    {
        private readonly SiaAdminDbContext _context;
        public DbSet<T> Table => _context.Set<T>();
        public StoredProcedureRepository(SiaAdminDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetProcedure(string proc)
        {
            var query = await Table.FromSqlRaw($"Exec {proc }").SingleOrDefaultAsync();
            return query;
        }

        public async Task<List<T>> GetProcedureList(string proc)
        {
            var query = await Table.FromSqlRaw($"Exec {proc}").AsNoTracking().ToListAsync();
            return query;
        }

        public async Task<List<T>> GetProcedureListWithDateRange(string proc, DateTime? startDate, DateTime? endDate)
        {
            string query="";
            if (startDate==null && endDate==null)
            {
                query = "exec " + proc + " null,null";
            }
            else
            {
                query = "Exec " + proc + " " + startDate + "," + endDate;
            }
            
            var list = await Table.FromSqlRaw($"{query}").ToListAsync();
            return list;
        }
    }
}
