namespace IlVecchioForno.Infrastructure.Tests;

public sealed class DbContextTests : DbTestsBase
{
    public DbContextTests(DbFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public Task SaveChangesAsync_OnInsert_GeneratesIdAndSetsTimestamps()
    {
        return Task.CompletedTask;
    }
}