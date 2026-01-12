using IlVecchioForno.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;

namespace IlVecchioForno.Infrastructure.Tests;

/// <summary>
///     Configuration for database integration tests using Testcontainers.
/// </summary>
/// <seealso href="https://dotnet.testcontainers.org/modules/postgres" />
/// <seealso href="https://xunit.net/docs/shared-context" />
public sealed class DbFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    public DbFixture()
    {
        this._container =
            new PostgreSqlBuilder($"{DbTestsConfig.ContainerImageName}:{DbTestsConfig.ContainerImageVersion}")
                .Build();
    }

    public async Task InitializeAsync()
    {
        await this._container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await this._container.DisposeAsync();
    }

    public IlVecchioFornoDbContext CreateTestDbContext()
    {
        string containerConnectionString = this._container.GetConnectionString();

        NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(containerConnectionString)
        {
            Database = $"test_{Guid.NewGuid():N}" // Same container, but different databases for each tests
        };

        DbContextOptions<IlVecchioFornoDbContext> options = new DbContextOptionsBuilder<IlVecchioFornoDbContext>()
            .UseNpgsql(builder.ConnectionString)
            .Options;

        return new IlVecchioFornoDbContext(options);
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DbFixture>;