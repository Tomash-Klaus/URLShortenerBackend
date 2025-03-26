using URLShortenerBackend.DTOs;
using URLShortenerBackend.Models;

namespace URLShortenerBackend.Services
{
    public interface IUrlService
    {
        public Task<UrlData> FindUrlDataByIdAsync(Guid id);

        public Task<bool> DeleteUrlAsync(Guid id);
        public Task<bool> CreateUrlRecord(string userId, string url);
        public Task<PagedResult<UrlData>> GetPagedUrlData(QueryParams queryParams);
        public Task<string> GetFullUrlIfExist(string shortUrl);
    }
}
