using Microsoft.AspNetCore.Identity;
using URLShortenerBackend.DTOs;
using URLShortenerBackend.Models;
using URLShortenerBackend.Repositories;

namespace URLShortenerBackend.Services.Implementations
{
    public class UrlService : IUrlService
    {
        private readonly IGeneralRepository<UrlData> _repo;
        private readonly IUrlRepository _urlRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUrlShortenerService _shortenerService;
        public UrlService(IGeneralRepository<UrlData> repo, IUrlShortenerService shortenerService, IUrlRepository urlRepo, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _repo = repo;
            _urlRepo = urlRepo;
            _shortenerService = shortenerService;
        }

        public async Task<bool> DeleteUrlAsync(Guid id)
        {
            var foundedUrlData = await _repo.GetByIdAsync(id);
            return foundedUrlData is null
                ? throw new BadHttpRequestException("Url is not found", StatusCodes.Status404NotFound)
                : await _repo.DeleteAsync(foundedUrlData);
        }

        public async Task<UrlData> FindUrlDataByIdAsync(Guid id)
        {
            var result = await _urlRepo.GetFullDataWithUserEmail(id);
            return result is null ? throw new BadHttpRequestException("Url is not found", StatusCodes.Status404NotFound) : result;
        }

        public async Task<string> GetFullUrlIfExist(string shortUrl)
        {
            var result = (await _repo.FindByConditionAsync(url => url.ShortUrl == shortUrl)).FirstOrDefault();
            return result is null ? throw new BadHttpRequestException("Url is not found", StatusCodes.Status404NotFound) : (result.FullUrl);
        }

        public async Task<bool> CreateUrlRecord(string userId, string url)
        {
            var shortenedUrl = _shortenerService.GenerateShortUrl(url);
            var findedUrl = await _repo.FindByConditionAsync(url=>url.ShortUrl== shortenedUrl);

            if(findedUrl.FirstOrDefault() is not null ) throw new BadHttpRequestException("There exists the same shortened URL", StatusCodes.Status409Conflict);

            var urlData = new UrlData
            {
                FullUrl = url,
                ShortUrl = shortenedUrl,
                CreatedBy =userId,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now)
            };

            return await _repo.AddAsync(urlData);
        }

        public async Task<PagedResult<UrlData>> GetPagedUrlData(QueryParams queryParams)
        {
            return await _repo.GetPagedDataAsync(queryParams);
        }
    }
}
