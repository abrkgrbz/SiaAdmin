using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Repositories
{
    public interface IStoredProcedureRepository<T> where T : class,new()
    {
        DbSet<T> Table { get; }
        Task<IEnumerable<T>> ExecuteStoredProcedureAsync(string procedureName, params SqlParameter[] parameters);
        Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(string procedureName, params SqlParameter[] parameters) where TResult : class, new();
        Task<int> ExecuteStoredProcedureNonQueryAsync(string procedureName, params SqlParameter[] parameters);
        Task<T> ExecuteStoredProcedureScalarAsync(string procedureName, params SqlParameter[] parameters); 
    
        Task<List<T>> GetProcedureListWithDateRange(string proc,DateTime? startDate,DateTime? endDate);
    }
}
