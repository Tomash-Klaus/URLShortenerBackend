using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using URLShortenerBackend.DTOs;

namespace URLShortenerBackend.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<string> LoginAsync(LoginDTO loginDTO)
        {
            var findedUser = await _userManager.FindByEmailAsync(loginDTO.Email) ?? throw new BadHttpRequestException("User not found", StatusCodes.Status403Forbidden);
            var userPassed = await _userManager.CheckPasswordAsync(findedUser, loginDTO.Password);

            if (!userPassed) throw new BadHttpRequestException("User pass is incorrect", StatusCodes.Status403Forbidden);

            var userRoles = await _userManager.GetRolesAsync(findedUser);

            return GenerateToken(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        private string GenerateToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? ""));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
