using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includePropeties = null, bool tracked = false);
        Task<T?> Get(Expression<Func<T, bool>> filter, string? includePropeties = null, bool tracked = false);
        Task Add(T entity);
        Task<bool> Any(Expression<Func<T,bool>> filter);
        Task Remove(T entity);
        Task Update(T entity);

    }
}
