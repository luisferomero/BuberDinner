using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace BuberDinner.Api.Middleware
{
    public static class ErrorHandlingMiddlewareExt
    {
        public static void UseErrorHandling(this WebApplication app)
        {
            // app.UseMiddleware<ErrorHandlingMiddleware>();

            app.Map("/error", (HttpContext httpContext) =>
                {
                    var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
                    var extensions = new Dictionary<string , object?>();
                    extensions["customPropertyFromMinimalAPI"] = "CustomValue from Minimal API";

                    return Results.Problem(title: exception?.Message, extensions: extensions);
                });
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
