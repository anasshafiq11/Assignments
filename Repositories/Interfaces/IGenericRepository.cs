
using Assignment2.Common.Querying;

namespace Assignment2.Repositories.Interfaces
{
    // This interface defines a generic repository pattern for performing CRUD operations on entities of type T.
    public interface IGenericRepository<T> where T: class
    {
        Task<List<T>> GetPagedAndFilteredAsync(QueryOptions queryOptions);
        Task<T> AddAsync(T entity);
        Task<T?> UpdateAsync(string id, T entity);
        Task<T?> GetByIdAsync(string id);
        Task<bool> DeleteAsync(string id);
    }
}
