using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Application.Repositories
{
    public interface IWriteRepository<T>: IRepository<T> where T:BaseEntity
    {
        Task<bool> AddAsync(T entity);  
        Task<T> AddAsyncReturnEntity(T entity);
        Task<bool> AddRangeAsync(List<T> entities);
        bool Remove(T entity);
        bool RemoveRange(List<T> datas);
        Task<bool> RemoveAsync(int id);
        bool Update(T entity);
        Task<int> SaveAsync(string userId=null,bool project=false);
        
    }
}
