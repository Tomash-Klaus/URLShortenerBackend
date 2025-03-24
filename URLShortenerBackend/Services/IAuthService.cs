using URLShortenerBackend.DTOs;

namespace URLShortenerBackend.Services
{
    public interface IAuthService
    {
        public Task<string> LoginAsync(LoginDTO loginDTO);
    }
}
