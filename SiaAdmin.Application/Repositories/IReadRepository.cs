using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Domain.Entities.Common;

namespace SiaAdmin.Application.Repositories
{
    public interface IReadRepository<T>:IRepository<T> where T:BaseEntity
    {
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
        Task<T> GetByIdAsync(int id, bool tracking = true);
        Task<int> GetCount(bool tracking = true);
        Task<IQueryable<T>> OrderByField<T>(IQueryable<T> q, string sortField, bool ascending);

    }
}
