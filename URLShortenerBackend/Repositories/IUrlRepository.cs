using URLShortenerBackend.Models;

namespace URLShortenerBackend.Repositories
{
    public interface IUrlRepository
    {
        public Task<UrlData?> GetFullDataWithUserEmail(Guid id);
    }
}
