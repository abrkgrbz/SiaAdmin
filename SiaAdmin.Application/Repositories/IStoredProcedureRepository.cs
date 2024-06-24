using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Repositories
{
    public interface IStoredProcedureRepository<T> where T : class,new()
    {
        Task<T> GetProcedure(string proc);
        Task<List<T>> GetProcedureList(string proc);
        Task<List<T>> GetProcedureListWithDateRange(string proc,DateTime? startDate,DateTime? endDate);
    }
}
