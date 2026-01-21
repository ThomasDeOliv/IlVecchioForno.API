using IlVecchioForno.Infrastructure.Persistence;

namespace IlVecchioForno.Infrastructure.Tests;

[Collection("Database")]
public abstract class DbTestsBase : IAsyncLifetime
{
    private readonly DbFixture _fixture;
    protected IlVecchioFornoDbContext _context;

    protected DbTestsBase(DbFixture fixture)
    {
        this._fixture = fixture;
        this._context = null!;
    }

    public Task InitializeAsync()
    {
        string dbName = GetRandomDbName();
        this._context = this._fixture.CreateTestDbContext(dbName);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await this._context.DisposeAsync();
        this._context = null!;
    }

    private static string GetRandomDbName()
    {
        return $"test_{Guid.NewGuid():N}";
    }
}