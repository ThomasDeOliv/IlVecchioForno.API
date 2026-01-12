using IlVecchioForno.Application;
using IlVecchioForno.Infrastructure;
using IlVecchioForno.Infrastructure.Persistence.Setup;

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
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();
        builder.Services.AddApplicationDependencies();
        builder.Services.AddInfrastructureDependencies(connectionString);

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        WebApplication app = builder.Build();

        await app.ApplyMigrationsAsync();
        await app.SeedDatabaseAsync();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) app.MapOpenApi();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}