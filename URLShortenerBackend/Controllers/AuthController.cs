using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLShortenerBackend.DTOs;
using URLShortenerBackend.Services;

namespace URLShortenerBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            return Ok(await _authService.LoginAsync(loginDTO));
        }


    }
}
