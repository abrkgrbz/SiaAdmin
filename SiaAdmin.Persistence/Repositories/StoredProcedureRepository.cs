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
    public class StoredProcedureRepository<T> : IStoredProcedureRepository<T> where T : class, new()
    {
        private readonly SiaAdminDbContext _context;
        public DbSet<T> Table => _context.Set<T>();
        public StoredProcedureRepository(SiaAdminDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync(string procedureName, params SqlParameter[] parameters)
        {

            return await _context.Set<T>().FromSqlRaw($"EXEC {procedureName} {GetParameterString(parameters)}",parameters).ToListAsync();
        }

        public async Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(string procedureName, params SqlParameter[] parameters) where TResult : class, new()
        {
            return await _context.Set<TResult>().FromSqlRaw($"EXEC {procedureName} {GetParameterString(parameters)}", parameters).ToListAsync();
        }

        public async Task<int> ExecuteStoredProcedureNonQueryAsync(string procedureName, params SqlParameter[] parameters)
        {
            return await _context.Database.ExecuteSqlRawAsync($"EXEC {procedureName} {GetParameterString(parameters)}");
        }

        public async Task<T> ExecuteStoredProcedureScalarAsync(string procedureName, params SqlParameter[] parameters)
        {
            return await Table.FromSqlRaw($"EXEC {procedureName} {GetParameterString(parameters)}", parameters).FirstOrDefaultAsync();
        }

        private string GetParameterString(SqlParameter[] parameters)
        {
            if (parameters == null || !parameters.Any())
                return string.Empty;
            return string.Join(", ", parameters.Select(p =>
                p.ParameterName.StartsWith("@") ? p.ParameterName : $"@{p.ParameterName}"));
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


            var list = await Table.FromSqlRaw($"{query}", @params).ToListAsync();
            return list;
        }
    }
}
