using IlVecchioForno.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests;

[Collection("Database")]
public class DbContextTests : IAsyncLifetime
{
    private readonly DbFixture _fixture;
    private IlVecchioFornoDbContext _context;

    public DbContextTests(DbFixture fixture)
    {
        this._fixture = fixture;
        this._context = null!;
    }

    public Task InitializeAsync()
    {
        this._context = this._fixture.CreateTestDbContext();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await this._context.DisposeAsync();
        this._context = null!;
    }

    [Fact]
    public async Task Database_CanBeCreated()
    {
        // Arrange & Act
        bool created = await this._context.Database.EnsureCreatedAsync();
        // Assert
        Assert.True(created);
    }

    [Fact]
    public async Task Migrations_ApplySuccessfully()
    {
        // Arrange & Act
        await this._context.Database.MigrateAsync();
        // Assert
        IEnumerable<string> pendingMigrations = await this._context.Database.GetPendingMigrationsAsync();
        IEnumerable<string> appliedMigrations = await this._context.Database.GetAppliedMigrationsAsync();
        Assert.Empty(pendingMigrations);
        Assert.NotEmpty(appliedMigrations);
    }

    [Fact]
    public async Task Schema_ExistAfterMigration()
    {
        // Arrange
        await this._context.Database.MigrateAsync();
        // Act
        List<string> schemas = await this._context.Database
            .SqlQuery<string>(
                $"""
                 SELECT schema_name
                 FROM information_schema.schemata;
                 """
            )
            .ToListAsync();
        // Assert
        Assert.NotEmpty(schemas);
        Assert.Equal(5, schemas.Count);
        Assert.Contains("pizzas_schema", schemas);
        Assert.Contains("public", schemas);
        Assert.Contains("information_schema", schemas);
        Assert.Contains("pg_catalog", schemas);
        Assert.Contains("pg_toast", schemas);
    }

    [Fact]
    public async Task Tables_ExistAfterMigration()
    {
        // Arrange
        await this._context.Database.MigrateAsync();
        // Act
        List<string> tables = await this._context.Database
            .SqlQuery<string>(
                $"""
                 SELECT table_name 
                 FROM information_schema.tables 
                 WHERE table_schema = 'pizzas_schema'
                 """
            )
            .ToListAsync();
        // Assert
        Assert.NotEmpty(tables);
        Assert.Equal(4, tables.Count);
        Assert.Contains("pizzas", tables);
        Assert.Contains("pizzas_ingredients", tables);
        Assert.Contains("ingredients", tables);
        Assert.Contains("quantity_types", tables);
    }
}