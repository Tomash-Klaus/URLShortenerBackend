using System.Linq.Expressions;

namespace URLShortenerBackend.Repositories
{
    public interface IGeneralRepository<T> where T : class
    {
        public Task<bool> AddAsync(T entity);
        public Task<bool> DeleteAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);
        public Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> predicate);
    }
}
