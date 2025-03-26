using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using URLShortenerBackend.Models;
using URLShortenerBackend.Repositories;

namespace URLShortenerBackend.Filters
{
    public class DeleteBasedOnRoleOrOwnRecordFilterAttribute : IAsyncActionFilter
    {
        private readonly IGeneralRepository<UrlData> _urlDataRepo;

        public DeleteBasedOnRoleOrOwnRecordFilterAttribute(IGeneralRepository<UrlData> urlDataRepo)
        {
            _urlDataRepo = urlDataRepo; 
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            var urlId = (Guid)context.ActionArguments["id"]!; 

            var urlData = await GetUrlDataById(urlId);  

            if (urlData is null)
            {
                context.Result = new NotFoundResult(); 
                return;
            }

            if (!user.IsInRole("Admin") && urlData.CreatedBy != user.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                context.Result = new ForbidResult(); 
                return;
            }

            await next();
        }

        private Task<UrlData?> GetUrlDataById(Guid id)
        {
            return _urlDataRepo.GetByIdAsync(id);
        }
    }
}
