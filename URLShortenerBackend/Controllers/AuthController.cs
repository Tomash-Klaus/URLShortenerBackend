using Microsoft.AspNetCore.Mvc;

namespace URLShortenerBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        

        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "Login")]
        public IActionResult Login()
        {
            return Ok();
        }
    }
}
