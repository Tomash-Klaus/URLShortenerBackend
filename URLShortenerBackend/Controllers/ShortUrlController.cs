using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLShortenerBackend.Services;

namespace URLShortenerBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShortUrlController : ControllerBase
    {

        private readonly IUrlService _urlService;
        public ShortUrlController(IUrlService urlService) {
            _urlService = urlService;
        }
        [AllowAnonymous]
        [HttpGet("{url}")]
        public async Task<IActionResult> RedirectToFullUrl(string url)
        {
            return Ok(await _urlService.GetFullUrlIfExist(url));
        }
    }
}
