using MassTransit;
using Microservices.Common.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Text.RegularExpressions;

namespace Microservices.Common.Exceptions
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public GlobalExceptionFilter(IPublishEndpoint publishEndpoint)
        {
            this._publishEndpoint = publishEndpoint;
        }
        public override async System.Threading.Tasks.Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is BadRequestException)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new JsonResult(new GlobalErrorVm
                {
                    ErrMsg = "Bad Request",
                    Detail = context.Exception.Message
                });
            }
            else if (context.Exception is DuplicateException)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status412PreconditionFailed;
                context.Result = new JsonResult(new GlobalErrorVm
                {
                    ErrMsg = "Bad Request",
                    Detail = context.Exception.Message
                });
            }
            else if (context.Exception is NotFoundException)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Result = new JsonResult(new GlobalErrorVm
                {
                    ErrMsg = "Not Found",
                    Detail = context.Exception.Message
                });
            }
            else if (context.Exception is DBConcurrencyException)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                context.Result = new JsonResult(new GlobalErrorVm
                {
                    ErrMsg = "Concurrent Request Error",
                    Detail = "The data your are trying to update is already updated. Please try again after re-fetch.",
                });
            }
            else if (context.Exception is DbUpdateException)
            {
                SqlException sqlException = (SqlException)context.Exception.InnerException;
                if (sqlException.Number == 547)
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                    context.Result = new JsonResult(new GlobalErrorVm
                    {
                        ErrMsg = "Database dependency error",
                        Detail = "Please resolve dependencies with " + Regex.Replace(sqlException.Message.Split('_')[1], "(\\B[A-Z])", " $1")
                    });
                }
                else
                {
                    context.Result = new JsonResult(new GlobalErrorVm
                    {
                        ErrMsg = "Internal Server Error",
                        Detail = context.Exception.Message
                    });
                    context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            }
            else
            {
                context.Result = new JsonResult(new GlobalErrorVm
                {
                    ErrMsg = "Internal Server Error",
                    Detail = context.Exception.Message
                });
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await _publishEndpoint.Publish(new GlobalExceptionMessage
                {
                    ApplicationName = context.ActionDescriptor.DisplayName,
                    ExceptionMessage = context.Exception?.Message,
                    InnerExceptionMessage = context.Exception?.InnerException?.Message,
                    OccurredAt = DateTimeOffset.UtcNow,
                    StackTrace = context.Exception?.StackTrace,
                    FunctionName = context.HttpContext.Request.Path.Value
                });
            }
        }
    }
}