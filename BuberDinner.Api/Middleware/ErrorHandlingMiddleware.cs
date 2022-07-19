using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BuberDinner.Api.Middleware
{
    public static class ErrorHandlingMiddlewareExt
    {
        public static void UseErrorHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = JsonSerializer.Serialize(new {error = "An error occurred while processing your request."});

            context.Response.ContentType = "application.json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
