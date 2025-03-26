using System.Linq.Expressions;
using URLShortenerBackend.DTOs;

namespace URLShortenerBackend.Repositories
{
    public interface IGeneralRepository<T> where T : class
    {
        public Task<bool> AddAsync(T entity);
        public Task<bool> DeleteAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(Guid id);
        public Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> predicate);
        public Task<PagedResult<T>> GetPagedDataAsync(QueryParams queryParams, Expression<Func<T, bool>>? filter = null);
    }
}
