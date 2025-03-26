using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using URLShortenerBackend.Data;
using URLShortenerBackend.DTOs;

namespace URLShortenerBackend.Repositories.Implementations
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : class
    {
        private readonly DbContextEF _context;

        // Конструктор для DI
        public GeneralRepository(DbContextEF context)
        {
            _context = context;
        }

        public DbSet<T> Entity => _context.Set<T>();

        public async Task<bool> AddAsync(T entity)
        {
            await Entity.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            Entity.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Entity.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.FindAsync<T>(id);
        }

        public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entity.Where(predicate).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<T>> GetPagedDataAsync(QueryParams queryParams, Expression<Func<T, bool>> filter = null)
        {
            var query = Entity.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return new PagedResult<T> { Items = items, PageNumber = queryParams.PageNumber, PageSize = queryParams.PageSize, TotalCount=totalCount };
        }
    }
}
