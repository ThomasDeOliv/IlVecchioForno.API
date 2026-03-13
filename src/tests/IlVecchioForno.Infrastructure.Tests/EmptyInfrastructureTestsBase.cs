using IlVecchioForno.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests;

[Collection("Database")]
public abstract class EmptyInfrastructureTestsBase : IAsyncLifetime
{
    private readonly DbContextFixture _dbCtxFixture;
    protected IlVecchioFornoDbContext _ctx;

    protected EmptyInfrastructureTestsBase(DbContextFixture dbCtxFixture)
    {
        this._dbCtxFixture = dbCtxFixture;
        this._ctx = null!;
    }

    public virtual async ValueTask DisposeAsync()
    {
        await this._ctx.Database.EnsureDeletedAsync(TestContext.Current.CancellationToken);
        await this._ctx.DisposeAsync();
        this._ctx = null!;
        GC.SuppressFinalize(this);
    }

    public virtual async ValueTask InitializeAsync()
    {
        string dbName = GetRandomDbName();
        this._ctx = this._dbCtxFixture.CreateTestDbContext(dbName);
        await this._ctx.Database.MigrateAsync(TestContext.Current.CancellationToken);
    }

    private static string GetRandomDbName()
    {
        return $"test_{Guid.NewGuid():N}";
    }
}