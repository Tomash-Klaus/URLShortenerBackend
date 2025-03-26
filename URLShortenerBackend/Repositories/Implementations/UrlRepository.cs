using Microsoft.EntityFrameworkCore;
using URLShortenerBackend.Data;
using URLShortenerBackend.Models;

namespace URLShortenerBackend.Repositories.Implementations
{
    public class UrlRepository : GeneralRepository<UrlData>, IUrlRepository
    {
        public UrlRepository(DbContextEF context) : base(context)
        {
        }
        
        public async Task<UrlData?> GetFullDataWithUserEmail(Guid id)
        {
          return await Entity.Where(data => data.Id == id).Include(u => u.User).FirstOrDefaultAsync();
        }
    }
}
