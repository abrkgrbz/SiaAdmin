using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
            SqlParameter[] @params = new SqlParameter[]
            {
                new SqlParameter()
                {
                    ParameterName = "@StartDate",
                    SqlDbType = System.Data.SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    Value = (object)startDate ?? DBNull.Value
                },
                new SqlParameter() {
                    ParameterName = "@EndDate",
                    SqlDbType =  System.Data.SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    Value = (object)endDate ?? DBNull.Value
                }   
            };
            string query = "Exec " + proc + " @StartDate, @EndDate";
           

            var list = await Table.FromSqlRaw($"{query}",@params).ToListAsync();
            return list;
        }
    }
}
