using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Response;

namespace Term7MovieApi.Filters
{
    public class ExceptionFilter: IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            var response = new ParentResponse();
            string message = context.Exception.Message;

            switch (context.Exception)
            {
                case DbNotFoundException:

                    _logger.LogDebug(message);

                    response.Message = Constants.MESSAGE_NOT_FOUND;
                    context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    context.Result = new JsonResult(response);
                    break;
                case DbOperationException:

                    if(message == "DBCONNECTION")
                    {
                        response.Message = "Cannot connect to data warehouse";
                        context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Result = new JsonResult(response);
                        break;
                    }

                    _logger.LogDebug(message);

                    response.Message = Constants.MESSAGE_BAD_REQUEST;
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Result = new JsonResult(response);
                    break;
                case DbForbiddenException:

                    _logger.LogDebug(message);

                    response.Message = Constants.MESSAGE_FORBIDDEN;
                    context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Result = new JsonResult(response);
                    break;
                case DbUpdateException:

                    _logger.LogDebug(message);

                    response.Message = Constants.MESSAGE_BAD_REQUEST;
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Result = new JsonResult(response);
                    break;

                case BadRequestException:

                    _logger.LogDebug(message);

                    response.Message = message;
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Result = new JsonResult(response);

                    break;
                case SqlException:

                    _logger.LogDebug($"{message}\n {context.Exception.StackTrace}\n");

                    response.Message = ErrorMessageConstants.ERROR_MESSAGE_DUPLICATE;
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Result = new JsonResult(response);

                    break;

                default:
                    _logger.LogError($"{context.Exception.Source}\n {message}\n {context.Exception.StackTrace}\n" );

                    response.Message = message;
                    context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Result = new JsonResult(response);
                    break;

            }
        }
    }
}
