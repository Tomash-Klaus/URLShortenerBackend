using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace URLShortenerBackend.Data
{
    public class DbContextEF(DbContextOptions<DbContextEF> options) : IdentityDbContext(options)
    {
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //}
    }
}
