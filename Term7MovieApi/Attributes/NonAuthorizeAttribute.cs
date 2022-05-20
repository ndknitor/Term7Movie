using Microsoft.AspNetCore.Mvc.Filters;
namespace Term7MovieApi;
public class NonAuthorizeAttribute : ActionFilterAttribute
{
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.HttpContext.Response.WriteAsync("{\"message\":\"User already authenticated\"}");
        }
        else
        {
            await next();
        }
    }
}