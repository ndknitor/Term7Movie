using Microsoft.AspNetCore.Mvc.Filters;

namespace Term7MovieApi
{
    public class CheckRequestForgeryAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string csrfTokenRequest = context.HttpContext.Request.Headers["X-CSRF-TOKEN"],
            csrfTokenSession = context.HttpContext.Session.GetString("CsrfToken");
            if (csrfTokenRequest == null || csrfTokenSession == null || csrfTokenRequest != csrfTokenSession)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.HttpContext.Response.WriteAsync("{\"message\":\"Need for CSRF validation\"}");
                context.HttpContext.Response.Body.Close();
            }
            else
            {
                await next();
            }
        }
    }
}