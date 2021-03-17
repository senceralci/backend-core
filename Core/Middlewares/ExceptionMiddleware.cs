using Core.Utilities.Messages;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Security;
using System.Threading.Tasks;

namespace Core.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = exception.Message;
            if (exception.GetType() == typeof(ValidationException))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exception.GetType() == typeof(ApplicationException))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exception.GetType() == typeof(UnauthorizedAccessException))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else if (exception.GetType() == typeof(SecurityException))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                message = ExceptionMessage.InternalServerError;
            }

            await httpContext.Response.WriteAsync(message);
        }
    }
}