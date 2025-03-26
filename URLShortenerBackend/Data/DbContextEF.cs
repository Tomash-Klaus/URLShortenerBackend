using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using URLShortenerBackend.Models;

namespace URLShortenerBackend.Data
{
    public class DbContextEF(DbContextOptions<DbContextEF> options) : IdentityDbContext(options)
    {
        public DbSet<UrlData> UrlDatas { get; set; }
    }
}
