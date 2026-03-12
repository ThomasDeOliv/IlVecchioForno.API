using IlVecchioForno.Infrastructure.Persistence;

namespace IlVecchioForno.Infrastructure.Tests;

[Collection("Database")]
public abstract class InfrastructureTestsBase : IAsyncLifetime
{
    private readonly DbContextFixture _dbCtxFixture;
    protected IlVecchioFornoDbContext _ctx;

    protected InfrastructureTestsBase(DbContextFixture dbCtxFixture)
    {
        this._dbCtxFixture = dbCtxFixture;
        this._ctx = null!;
    }

    public Task InitializeAsync()
    {
        string dbName = GetRandomDbName();
        this._ctx = this._dbCtxFixture.CreateTestDbContext(dbName);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await this._ctx.DisposeAsync();
        this._ctx = null!;
    }

    private static string GetRandomDbName()
    {
        return $"test_{Guid.NewGuid():N}";
    }
}