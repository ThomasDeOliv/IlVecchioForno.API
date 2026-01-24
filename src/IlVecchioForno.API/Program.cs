using IlVecchioForno.Application;
using IlVecchioForno.Infrastructure;
using IlVecchioForno.Infrastructure.Persistence.Setup;
using Scalar.AspNetCore;

namespace IlVecchioForno.API;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }
}