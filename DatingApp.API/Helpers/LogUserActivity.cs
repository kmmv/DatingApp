using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        // two paramters - ActionExecutingContext (which means action being executed)
        // ActionExecutionDelegate - after the action has been executed
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // we can run during or after but we are using after
            // inside the resultcontext will be type of action executed context
            // resultcontext will have httpContext
            var resultContext = await next();

            // get userId from token
            var userId = int.Parse(resultContext.HttpContext.User
               .FindFirst(ClaimTypes.NameIdentifier).Value);
            
            // GetService provided as a dependecy injection container in our startup class
            // GetService requires Microsoft.Extensions.DependencyInjection;
            var repo = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();
            // bring user from userId
            var user = await repo.GetUser(userId);
            // update LastActive
            user.LastActive = DateTime.Now;
            // Save back to database
            await repo.SaveAll();
        }
    }
}