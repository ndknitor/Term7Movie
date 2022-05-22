using Microsoft.AspNetCore.Mvc.Filters;
using Term7MovieApi.Constants;
using Term7MovieApi.Extensions;
using Term7MovieApi.Responses;

namespace Term7MovieApi;
public class NonAuthorizeAttribute : ActionFilterAttribute
{
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            var res = new ParentResponse() 
            { 
                Message = MessageConstants.MESSAGE_AUTHORIZED 
            };
            await context.HttpContext.Response.WriteAsync(res.ToJson());
        }
        else
        {
            await next();
        }
    }
}