using IlVecchioForno.Application;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Infrastructure;
using IlVecchioForno.Infrastructure.Persistence.Setup;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Fetch connection string
        string? connectionString =
            builder.Configuration.GetConnectionString(InfrastructureStartup.ConnectionStringName);
        ArgumentNullException.ThrowIfNull(connectionString);

        // Add services to the container.
        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options => // TODO : move in an other directory
            {
                options.InvalidModelStateResponseFactory = _ => new BadRequestObjectResult(
                    new
                    {
                        Success = false,
                        Type = ResultType.ValidationError,
                        ErrorMessage = "Invalid request model provided.",
                        Content = (object?)null
                    }
                );
            });

        builder.Services.AddAuthorization();
        builder.Services.AddApplicationDependencies();
        builder.Services.AddInfrastructureDependencies(connectionString);

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        WebApplication app = builder.Build();

        await app.ApplyMigrationsAsync();
        await app.SeedDatabaseAsync();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
            app.MapOpenApi();
        else
            app.UseExceptionHandler(options =>
            {
                options.Run(async context => // TODO : move in an other directory
                {
                    ILoggerFactory loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                    ILogger logger = loggerFactory.CreateLogger("UnhandledException");

                    Exception? exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                    logger.LogError(exception, "Unhandled exception");

                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsJsonAsync(new
                    {
                        Success = false,
                        Type = ResultType.InternalError,
                        ErrorMessage = "Internal server error",
                        Content = (object?)null
                    });
                });
            });

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}