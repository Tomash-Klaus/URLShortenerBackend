using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Security.Claims;

namespace URLShortenerBackend.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>();
            var controllerName = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ControllerName;

            var isAuthController = string.Equals(controllerName, "Auth", StringComparison.OrdinalIgnoreCase);

            if (allowAnonymous != null || isAuthController)
            {
                await _next(context);
                return;
            }

            var user = context.User;

            if (user?.Identity?.IsAuthenticated != true ||
                !user.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier))
            {
                throw new BadHttpRequestException("Token does not contain user id", StatusCodes.Status403Forbidden);
            }

            await _next(context);

        }
    }
}
