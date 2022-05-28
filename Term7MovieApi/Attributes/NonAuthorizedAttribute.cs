using Microsoft.AspNetCore.Mvc.Filters;
using Term7MovieCore.Extensions;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Response;

namespace Term7MovieApi;
public class NonAuthorizedAttribute : ActionFilterAttribute
{
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            var res = new ParentResponse() 
            { 
                Message = Constants.MESSAGE_AUTHORIZED 
            };
            await context.HttpContext.Response.WriteAsync(res.ToJson());
        }
        else
        {
            await next();
        }
    }
}