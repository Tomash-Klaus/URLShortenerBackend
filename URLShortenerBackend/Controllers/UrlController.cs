using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using URLShortenerBackend.DTOs;
using URLShortenerBackend.Filters;
using URLShortenerBackend.Services;

namespace URLShortenerBackend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;
        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryParams queryParams)
        {
            return Ok(await _urlService.GetPagedUrlData(queryParams));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUrlById(Guid id)
        {
            return Ok(await _urlService.FindUrlDataByIdAsync(id));
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(DeleteBasedOnRoleOrOwnRecordFilterAttribute))]
        public async Task<IActionResult> DeleteUrl(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!; 
            return Ok(await _urlService.DeleteUrlAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUrlAsync([FromBody] CreateUrlDTO createUrlDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!; 
           
            return Ok(await _urlService.CreateUrlRecord(userId, createUrlDTO.FullUrl));
        }
    }
}
