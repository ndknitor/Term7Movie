using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Response;

namespace Term7MovieApi.Filters
{
    public class ExceptionFilter: IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = new ParentResponse();

            switch (context.Exception)
            {
                case DbNotFoundException:
                    response.Message = Constants.MESSAGE_NOT_FOUND;
                    context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    context.Result = new JsonResult(response);
                    break;
                case DbOperationException:
                    response.Message = Constants.MESSAGE_OPERATION_FAILED;
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Result = new JsonResult(response);
                    break;
                case DbForbiddenException:
                    response.Message = Constants.MESSAGE_FORBIDDEN;
                    context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Result = new JsonResult(response);
                    break;

            }
        }
    }
}
