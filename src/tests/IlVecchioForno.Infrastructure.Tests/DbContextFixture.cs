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
public sealed class DbContextFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    public DbContextFixture()
    {
        this._container =
            new PostgreSqlBuilder($"{DbTestsConfig.ContainerImageName}:{DbTestsConfig.ContainerImageVersion}")
                .Build();
    }

    public async ValueTask InitializeAsync()
    {
        await this._container.StartAsync(TestContext.Current.CancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await this._container.DisposeAsync();
    }

    public IlVecchioFornoDbContext CreateTestDbContext(string dbName)
    {
        return this.CreateTestDbContext(dbName, TimeProvider.System);
    }

    private IlVecchioFornoDbContext CreateTestDbContext(string dbName, TimeProvider timeProvider)
    {
        string containerConnectionString = this._container.GetConnectionString();

        NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(containerConnectionString)
        {
            Database = dbName // Same container, but different databases for each tests
        };

        DbContextOptions<IlVecchioFornoDbContext> options = new DbContextOptionsBuilder<IlVecchioFornoDbContext>()
            .UseNpgsql(builder.ConnectionString)
            .Options;

        return new IlVecchioFornoDbContext(options, timeProvider);
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DbContextFixture>;