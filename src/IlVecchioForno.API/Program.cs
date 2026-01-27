using IlVecchioForno.API.Cors;
using IlVecchioForno.API.Logger;
using IlVecchioForno.Application;
using IlVecchioForno.Infrastructure;
using IlVecchioForno.Infrastructure.Persistence.Setup;
using Scalar.AspNetCore;
using Serilog;

namespace IlVecchioForno.API;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        SerilogConfiguration.ConfigureSerilog(builder.Configuration);

        try
        {
            Log.Information("Starting application.");

            builder.Host
                .UseSerilog();

            builder.Services
                .AddApiDependencies()
                .AddApplicationDependencies()
                .AddInfrastructureDependencies(
                    builder.Configuration.GetConnectionString(InfrastructureStartup.ConnectionStringName)
                );

            WebApplication app = builder.Build();

            await app.ApplyMigrationsAsync();
            await app.SeedDatabaseAsync();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly.");
        }
        finally
        {
            Log.Information("Application shutting down."); 
            await Log.CloseAndFlushAsync();
        }
    }
}