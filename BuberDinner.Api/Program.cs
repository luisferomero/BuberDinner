
using BuberDinner.Api.Filters;
using BuberDinner.Api.Middleware;
using BuberDinner.Application;
using BuberDinner.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    // builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
    builder.Services.AddControllers();
}

var app = builder.Build();
{
    // app.UseErrorHandling();
    app.UseExceptionHandler("/error");

    app.Map("/error", (HttpContext httpContext) =>
    {
        var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        var extensions = new Dictionary<string , object?>()
        {
            {"Custom property", "CustomValue"}
        };

        return Results.Problem(title: exception?.Message, extensions: extensions);
    });

    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}

