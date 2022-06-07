﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Response;

namespace Term7MovieApi.Filters
{
    public class ExceptionFilter: IExceptionFilter
    {
        private readonly ILogger _logger;
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

                    _logger.LogError(message);

                    response.Message = Constants.MESSAGE_BAD_REQUEST;
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Result = new JsonResult(response);
                    break;

            }
        }
    }
}
