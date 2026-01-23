using IlVecchioForno.API.Filters;
using IlVecchioForno.API.Presenters;
using IlVecchioForno.API.Resources.ActivePizza;
using IlVecchioForno.API.Resources.ArchivedPizza;
using IlVecchioForno.API.Resources.Ingredient;
using IlVecchioForno.API.Resources.QuantityType;
using IlVecchioForno.Application;
using IlVecchioForno.Application.UseCases.Ingredients.DTOs;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;
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
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<GlobalExceptionFilter>();
        builder.Services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
        builder.Services.AddScoped<IPresenter<ActivePizzaDto, ActivePizzaResource>, ActivePizzaPresenter>();
        builder.Services.AddScoped<IPresenter<ArchivedPizzaDto, ArchivedPizzaResource>, ArchivedPizzaPresenter>();
        builder.Services.AddScoped<IPresenter<IngredientDto, IngredientResource>, IngredientPresenter>();
        builder.Services.AddScoped<IPresenter<QuantityTypeDto, QuantityTypeResource>, QuantityTypePresenter>();
        builder.Services.AddInfrastructureDependencies(connectionString);
        builder.Services.AddApplicationDependencies();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        WebApplication app = builder.Build();

        await app.ApplyMigrationsAsync();
        await app.SeedDatabaseAsync();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
            app.MapOpenApi();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}