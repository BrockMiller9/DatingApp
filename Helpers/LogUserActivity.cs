
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next(); // this is the action being executed

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId(); // this will get the username from the token

            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>(); // this will get the user repository

            var user = await repo.GetUserByIdAsync(int.Parse(userId)); // this will get the user from the database

            user.LastActive = DateTime.UtcNow; // this will set the last active property to the current date and time

            await repo.SaveAllAsync(); // this will save the changes to the database
        }
    }
}