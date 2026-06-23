using Gridify;
using System.Linq.Expressions;
using UsersApi.Common;
using UsersApi.Common.Querying;

namespace UsersApi.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetPagedAndFilteredAsync(QueryOptions queryOptions);
        Task<T> AddAsync(T entity);
        Task<T?> UpdateAsync(int id, T entity);
        Task<T?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
