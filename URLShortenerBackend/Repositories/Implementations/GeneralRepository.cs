using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using URLShortenerBackend.Data;

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

        public async Task<T?> GetByIdAsync(int id)
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
    }
}
