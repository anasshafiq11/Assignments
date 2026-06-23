using Gridify;
using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using UserApi.Data;
using UsersApi.Common;
using UsersApi.Common.Helpers;
using UsersApi.Common.Querying;
using UsersApi.Models;
using UsersApi.Repositories.Interfaces;

namespace UsersApi.Repositories
{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetPagedAndFilteredAsync(QueryOptions queryOptions)
        {
            IQueryable<T> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(queryOptions.FilterExpression) && QueryValidator.IsValidFilterExpression<T>(queryOptions.FilterExpression))
            {

                query = query.Where(queryOptions.FilterExpression, queryOptions.Parameters);
            }

            if (!string.IsNullOrWhiteSpace(queryOptions.OrderByExpression) && QueryValidator.IsValidProperty<T>(queryOptions.OrderByExpression))
            {
                query = query.OrderBy(queryOptions.OrderByExpression);
            }
            else
            {
                query = query.OrderBy("Id");
            }

            return await query.Skip(queryOptions.Skip).Take(queryOptions.Take).ToListAsync();
            // return await query.Where(queryOptions.FilterExpression, queryOptions.Parameters).OrderBy(queryOptions.OrderByExpression).Skip(queryOptions.Skip).Take(queryOptions.Take).ToListAsync();
        }
       
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<T?> UpdateAsync(int id, T entity)
        {
            var existing = await GetByIdAsync(id);
            if (existing == null) return null;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return existing;
        }

    }
}
