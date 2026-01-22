using IlVecchioForno.Domain.QuantityTypes;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests;

public sealed class DbContextTests : DbTestsBase
{
    public DbContextTests(DbFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task SaveChangesAsync_OnInsert_GeneratesIdAndSetsTimestamps()
    {
        // Arrange
        await this._context.Database.MigrateAsync();
        QuantityType quantityType = new QuantityType(
            new QuantityTypeName("Gram"),
            new QuantityTypeUnit("g")
        );

        int idBefore = quantityType.Id;
        string nameBefore = quantityType.Name.Value;
        string? unitBefore = quantityType.Unit?.Value;
        DateTime? createdAtBefore = quantityType.CreatedAt;
        DateTime? updatedAtBefore = quantityType.UpdatedAt;

        // Act
        this._context.QuantityTypes.Add(quantityType);
        await this._context.SaveChangesAsync();

        int idAfter = quantityType.Id;
        string nameAfter = quantityType.Name.Value;
        string? unitAfter = quantityType.Unit?.Value;
        DateTime? createdAtAfter = quantityType.CreatedAt;
        DateTime? updatedAtAfter = quantityType.UpdatedAt;

        // Assert => Before: ID not set, timestamps at MinValue
        Assert.Equal(0, idBefore);
        Assert.Equal(DateTime.MinValue, createdAtBefore);
        Assert.Equal(DateTime.MinValue, updatedAtBefore);

        // Assert => After: ID generated, timestamps set
        Assert.Equal(1, idAfter);
        Assert.NotEqual(DateTime.MinValue, createdAtAfter);
        Assert.NotEqual(DateTime.MinValue, updatedAtAfter);

        // Assert => Business data unchanged
        Assert.Equal(nameBefore, nameAfter);
        Assert.Equal(unitBefore, unitAfter);
    }
}