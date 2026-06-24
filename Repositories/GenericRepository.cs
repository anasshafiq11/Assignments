using Assignment2.Common.Querying;
using Assignment2.Data;
using Assignment2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using UserManagement.Common.Helpers;

namespace Assignment2.Repositories
{
    public class GenericRepository<T>: IGenericRepository<T> where T: class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetPagedAndFilteredAsync(QueryOptions queryOptions)
        {
            IQueryable<T> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(queryOptions.FilterExpression))
            {
                try
                {
                    query = query.Where(queryOptions.FilterExpression, queryOptions.Parameters);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Invalid filter expression: {ex.Message}");
                }
            }

            if (!string.IsNullOrWhiteSpace(queryOptions.OrderByExpression))
            {
                try
                {
                    query = query.OrderBy(queryOptions.OrderByExpression);
                }
                catch (Exception)
                {
                    query = query.OrderBy("Id");
                }
            }
            return await query.Skip(queryOptions.Skip).Take(queryOptions.Take).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<T?> UpdateAsync(string id, T entity)
        {
            var existing = await GetByIdAsync(id);
            if (existing == null) return null;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return existing;
        }



    }
}
