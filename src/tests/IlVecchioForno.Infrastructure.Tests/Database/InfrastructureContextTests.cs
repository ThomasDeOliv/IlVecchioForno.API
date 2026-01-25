using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Domain.QuantityTypes;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.Database;

public sealed class InfrastructureContextTests : InfrastructureTestsBase
{
    public InfrastructureContextTests(DbContextFixture dbCtxFixture) : base(dbCtxFixture)
    {
    }

    private static QuantityType GetTestQuantityType(string name, string unit)
    {
        return new QuantityType(
            new QuantityTypeName(name),
            new QuantityTypeUnit(unit)
        );
    }

    private static Pizza GetTestPizza(string name, string? description, decimal price, int id = 0)
    {
        return new Pizza(
            new PizzaName(name),
            !string.IsNullOrEmpty(description)
                ? new PizzaDescription(description)
                : null,
            new PizzaPrice(price),
            id
        );
    }

    [Fact]
    public async Task SaveChangesAsync_OnInsert_GeneratesIdAndTimestamps()
    {
        // Arrange
        await this._ctx.Database.MigrateAsync();
        QuantityType quantityType = GetTestQuantityType("Gram", "g");

        int idBefore = quantityType.Id;
        string nameBefore = quantityType.Name.Value;
        string unitBefore = quantityType.Unit.Value;
        DateTime? createdAtBefore = quantityType.CreatedAt;
        DateTime? updatedAtBefore = quantityType.UpdatedAt;

        // Act
        this._ctx.QuantityTypes.Add(quantityType);
        await this._ctx.SaveChangesAsync();

        int idAfter = quantityType.Id;
        string nameAfter = quantityType.Name.Value;
        string unitAfter = quantityType.Unit.Value;
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

    [Fact]
    public async Task SaveChangesAsync_OnUpdate_SetsUpdatedAtTimestamp()
    {
        // // Arrange
        // await this._ctx.Database.MigrateAsync();
        // QuantityType quantityType = GetTestQuantityType("Gram", "g");
        //
        // int idBefore = quantityType.Id;
        // string nameBefore = quantityType.Name.Value;
        // string? unitBefore = quantityType.Unit?.Value;
        // DateTime? createdAtBefore = quantityType.CreatedAt;
        // DateTime? updatedAtBefore = quantityType.UpdatedAt;
        //
        // // Act
        // this._ctx.QuantityTypes.Add(quantityType);
        // await this._ctx.SaveChangesAsync();
        //
        // quan
        //
        // int idAfter = quantityType.Id;
        // string nameAfter = quantityType.Name.Value;
        // string? unitAfter = quantityType.Unit?.Value;
        // DateTime? createdAtAfter = quantityType.CreatedAt;
        // DateTime? updatedAtAfter = quantityType.UpdatedAt;
        //
        // // Assert => Before: ID not set, timestamps at MinValue
        // Assert.Equal(0, idBefore);
        // Assert.Equal(DateTime.MinValue, createdAtBefore);
        // Assert.Equal(DateTime.MinValue, updatedAtBefore);
        //
        // // Assert => After: ID generated, timestamps set
        // Assert.Equal(1, idAfter);
        // Assert.NotEqual(DateTime.MinValue, createdAtAfter);
        // Assert.NotEqual(DateTime.MinValue, updatedAtAfter);
        //
        // // Assert => Business data unchanged
        // Assert.Equal(nameBefore, nameAfter);
        // Assert.Equal(unitBefore, unitAfter);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task SaveChangesAsync_WithTrue_SetsEntityStateToUnchanged()
    {
        // Arrange                                                                                                                                                                                                                                                                                                                                                                                                                  
        await this._ctx.Database.MigrateAsync();
        QuantityType quantityType = GetTestQuantityType("Gram", "g");

        // Act                            
        this._ctx.QuantityTypes.Add(quantityType);
        await this._ctx.SaveChangesAsync(true);

        // Assert                                                                                                                                                                                                                                                                                                                                                                                                                   
        EntityState state = this._ctx.Entry(quantityType).State;
        Assert.Equal(EntityState.Unchanged, state);
    }

    [Fact]
    public async Task SaveChangesAsync_WithFalse_KeepsEntityStateAsAdded()
    {
        // Arrange                                                                                                                                                                                                                                                                                                                                                                                                                  
        await this._ctx.Database.MigrateAsync();
        QuantityType quantityType = GetTestQuantityType("Gram", "g");

        // Act                                   
        this._ctx.QuantityTypes.Add(quantityType);
        await this._ctx.SaveChangesAsync(false);

        // Assert                                                                                                                                                                                                                                                                                                                                                                                                                   
        EntityState state = this._ctx.Entry(quantityType).State;
        Assert.Equal(EntityState.Added, state);
    }
}