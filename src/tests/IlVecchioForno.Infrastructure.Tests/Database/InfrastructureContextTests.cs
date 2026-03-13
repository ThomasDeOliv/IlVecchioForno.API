using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Domain.QuantityTypes;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.Database;

public sealed class InfrastructureContextTests : EmptyInfrastructureTestsBase
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
    public void SaveChanges_OnInsert_GeneratesIdAndTimestampsAndSetsStateToUnchanged()
    {
        // Arrange
        QuantityType quantityType = GetTestQuantityType("Gram", "g");

        EntityState stateBeforeInsert = this._ctx.Entry(quantityType).State;
        int idBeforeInsert = quantityType.Id;
        DateTime createdAtBeforeInsert = quantityType.CreatedAt;
        DateTime updatedAtBeforeInsert = quantityType.UpdatedAt;

        // Act
        this._ctx.QuantityTypes.Add(quantityType);

        EntityState stateAfterInsert = this._ctx.Entry(quantityType).State;
        int idAfterInsert = quantityType.Id;
        DateTime createdAtAfterInsert = quantityType.CreatedAt;
        DateTime updatedAtAfterInsert = quantityType.UpdatedAt;

        this._ctx.SaveChanges();

        EntityState stateAfterSaveChanges = this._ctx.Entry(quantityType).State;
        int idAfterSaveChanges = quantityType.Id;
        DateTime createdAtAfterSaveChanges = quantityType.CreatedAt;
        DateTime updatedAtAfterSaveChanges = quantityType.UpdatedAt;

        // Assert
        Assert.Equal(EntityState.Detached, stateBeforeInsert);
        Assert.Equal(0, idBeforeInsert);
        Assert.Equal(DateTime.MinValue, createdAtBeforeInsert);
        Assert.Equal(DateTime.MinValue, updatedAtBeforeInsert);

        Assert.Equal(EntityState.Added, stateAfterInsert);
        Assert.Equal(0, idAfterInsert);
        Assert.Equal(DateTime.MinValue, createdAtAfterInsert);
        Assert.Equal(DateTime.MinValue, updatedAtAfterInsert);

        Assert.Equal(EntityState.Unchanged, stateAfterSaveChanges);
        Assert.Equal(1, idAfterSaveChanges);
        Assert.NotEqual(DateTime.MinValue, createdAtAfterSaveChanges);
        Assert.NotEqual(DateTime.MinValue, updatedAtAfterSaveChanges);
    }

    [Fact]
    public async Task SaveChangesAsync_OnInsert_GeneratesIdAndTimestampsAndSetsStateToUnchanged()
    {
        // Arrange
        QuantityType quantityType = GetTestQuantityType("Gram", "g");

        EntityState stateBeforeInsert = this._ctx.Entry(quantityType).State;
        int idBeforeInsert = quantityType.Id;
        DateTime createdAtBeforeInsert = quantityType.CreatedAt;
        DateTime updatedAtBeforeInsert = quantityType.UpdatedAt;

        // Act
        this._ctx.QuantityTypes.Add(quantityType);

        EntityState stateAfterInsert = this._ctx.Entry(quantityType).State;
        int idAfterInsert = quantityType.Id;
        DateTime createdAtAfterInsert = quantityType.CreatedAt;
        DateTime updatedAtAfterInsert = quantityType.UpdatedAt;

        await this._ctx.SaveChangesAsync(TestContext.Current.CancellationToken);

        EntityState stateAfterSaveChanges = this._ctx.Entry(quantityType).State;
        int idAfterSaveChanges = quantityType.Id;
        DateTime createdAtAfterSaveChanges = quantityType.CreatedAt;
        DateTime updatedAtAfterSaveChanges = quantityType.UpdatedAt;

        // Assert
        Assert.Equal(EntityState.Detached, stateBeforeInsert);
        Assert.Equal(0, idBeforeInsert);
        Assert.Equal(DateTime.MinValue, createdAtBeforeInsert);
        Assert.Equal(DateTime.MinValue, updatedAtBeforeInsert);

        Assert.Equal(EntityState.Added, stateAfterInsert);
        Assert.Equal(0, idAfterInsert);
        Assert.Equal(DateTime.MinValue, createdAtAfterInsert);
        Assert.Equal(DateTime.MinValue, updatedAtAfterInsert);

        Assert.Equal(EntityState.Unchanged, stateAfterSaveChanges);
        Assert.Equal(1, idAfterSaveChanges);
        Assert.NotEqual(DateTime.MinValue, createdAtAfterSaveChanges);
        Assert.NotEqual(DateTime.MinValue, updatedAtAfterSaveChanges);
    }

    [Fact]
    public void SaveChanges_OnUpdate_SetsUpdatedAtAndSetsStateToUnchanged()
    {
        // Arrange
        Pizza pizza = GetTestPizza("Gorgonzola", null, 12.50m);
        this._ctx.Pizzas.Add(pizza);
        this._ctx.SaveChanges();

        EntityState stateBeforeUpdate = this._ctx.Entry(pizza).State;
        int idBeforeUpdate = pizza.Id;
        DateTime createdAtBeforeUpdate = pizza.CreatedAt;
        DateTime updatedAtBeforeUpdate = pizza.UpdatedAt;

        // Act
        pizza.UpdateDescription(new PizzaDescription("A pizza description."));

        EntityState stateAfterUpdate = this._ctx.Entry(pizza).State;
        int idAfterUpdate = pizza.Id;
        DateTime createdAtAfterUpdate = pizza.CreatedAt;
        DateTime updatedAtAfterUpdate = pizza.UpdatedAt;

        this._ctx.SaveChanges();

        EntityState stateAfterSaveChanges = this._ctx.Entry(pizza).State;
        int idAfterSaveChanges = pizza.Id;
        DateTime createdAtAfterSaveChanges = pizza.CreatedAt;
        DateTime updatedAtAfterSaveChanges = pizza.UpdatedAt;

        // Assert
        Assert.Equal(EntityState.Unchanged, stateBeforeUpdate);
        Assert.Equal(1, idBeforeUpdate);
        Assert.NotEqual(DateTime.MinValue, createdAtBeforeUpdate);
        Assert.NotEqual(DateTime.MinValue, updatedAtBeforeUpdate);

        Assert.Equal(EntityState.Modified, stateAfterUpdate);
        Assert.Equal(idBeforeUpdate, idAfterUpdate);
        Assert.Equal(createdAtBeforeUpdate, createdAtAfterUpdate);
        Assert.Equal(updatedAtBeforeUpdate, updatedAtAfterUpdate);

        Assert.Equal(EntityState.Unchanged, stateAfterSaveChanges);
        Assert.Equal(idAfterUpdate, idAfterSaveChanges);
        Assert.Equal(createdAtAfterUpdate, createdAtAfterSaveChanges);
        Assert.NotEqual(updatedAtAfterUpdate, updatedAtAfterSaveChanges);
    }

    [Fact]
    public async Task SaveChangesAsync_OnUpdate_SetsUpdatedAtAndSetsStateToUnchanged()
    {
        // Arrange
        Pizza pizza = GetTestPizza("Gorgonzola", null, 12.50m);
        this._ctx.Pizzas.Add(pizza);
        await this._ctx.SaveChangesAsync(TestContext.Current.CancellationToken);

        EntityState stateBeforeUpdate = this._ctx.Entry(pizza).State;
        int idBeforeUpdate = pizza.Id;
        DateTime createdAtBeforeUpdate = pizza.CreatedAt;
        DateTime updatedAtBeforeUpdate = pizza.UpdatedAt;

        // Act
        pizza.UpdateDescription(new PizzaDescription("A pizza description."));

        EntityState stateAfterUpdate = this._ctx.Entry(pizza).State;
        int idAfterUpdate = pizza.Id;
        DateTime createdAtAfterUpdate = pizza.CreatedAt;
        DateTime updatedAtAfterUpdate = pizza.UpdatedAt;

        await this._ctx.SaveChangesAsync(TestContext.Current.CancellationToken);

        EntityState stateAfterSaveChanges = this._ctx.Entry(pizza).State;
        int idAfterSaveChanges = pizza.Id;
        DateTime createdAtAfterSaveChanges = pizza.CreatedAt;
        DateTime updatedAtAfterSaveChanges = pizza.UpdatedAt;

        // Assert
        Assert.Equal(EntityState.Unchanged, stateBeforeUpdate);
        Assert.Equal(1, idBeforeUpdate);
        Assert.NotEqual(DateTime.MinValue, createdAtBeforeUpdate);
        Assert.NotEqual(DateTime.MinValue, updatedAtBeforeUpdate);

        Assert.Equal(EntityState.Modified, stateAfterUpdate);
        Assert.Equal(idBeforeUpdate, idAfterUpdate);
        Assert.Equal(createdAtBeforeUpdate, createdAtAfterUpdate);
        Assert.Equal(updatedAtBeforeUpdate, updatedAtAfterUpdate);

        Assert.Equal(EntityState.Unchanged, stateAfterSaveChanges);
        Assert.Equal(idAfterUpdate, idAfterSaveChanges);
        Assert.Equal(createdAtAfterUpdate, createdAtAfterSaveChanges);
        Assert.NotEqual(updatedAtAfterUpdate, updatedAtAfterSaveChanges);
    }

    [Fact]
    public void SaveChanges_WithFalse_OnInsert_SetsTimestampsButKeepsStateAsAdded()
    {
        // Arrange                                 
        QuantityType quantityType = GetTestQuantityType("Gram", "g");

        EntityState stateBeforeInsert = this._ctx.Entry(quantityType).State;

        int idBeforeInsert = quantityType.Id;
        DateTime createdAtBeforeInsert = quantityType.CreatedAt;
        DateTime updatedAtBeforeInsert = quantityType.UpdatedAt;

        // Act                                   
        this._ctx.QuantityTypes.Add(quantityType);

        EntityState stateAfterInsert = this._ctx.Entry(quantityType).State;
        int idAfterInsert = quantityType.Id;
        DateTime createdAtAfterInsert = quantityType.CreatedAt;
        DateTime updatedAtAfterInsert = quantityType.UpdatedAt;

        this._ctx.SaveChanges(false);

        EntityState stateAfterSaveChanges = this._ctx.Entry(quantityType).State;
        int idAfterSaveChanges = quantityType.Id;
        DateTime createdAtAfterSaveChanges = quantityType.CreatedAt;
        DateTime updatedAtAfterSaveChanges = quantityType.UpdatedAt;

        // Assert                      
        Assert.Equal(EntityState.Detached, stateBeforeInsert);
        Assert.Equal(0, idBeforeInsert);
        Assert.Equal(DateTime.MinValue, createdAtBeforeInsert);
        Assert.Equal(DateTime.MinValue, updatedAtBeforeInsert);

        Assert.Equal(EntityState.Added, stateAfterInsert);
        Assert.Equal(0, idAfterInsert);
        Assert.Equal(DateTime.MinValue, createdAtAfterInsert);
        Assert.Equal(DateTime.MinValue, updatedAtAfterInsert);

        Assert.Equal(EntityState.Added, stateAfterSaveChanges);
        Assert.Equal(0, idAfterSaveChanges);
        Assert.NotEqual(DateTime.MinValue, createdAtAfterSaveChanges);
        Assert.NotEqual(DateTime.MinValue, updatedAtAfterSaveChanges);
    }

    [Fact]
    public async Task SaveChangesAsync_WithFalse_OnInsert_SetsTimestampsButKeepsStateAsAdded()
    {
        // Arrange                                    
        QuantityType quantityType = GetTestQuantityType("Gram", "g");

        EntityState stateBeforeInsert = this._ctx.Entry(quantityType).State;

        int idBeforeInsert = quantityType.Id;
        DateTime createdAtBeforeInsert = quantityType.CreatedAt;
        DateTime updatedAtBeforeInsert = quantityType.UpdatedAt;

        // Act                                   
        this._ctx.QuantityTypes.Add(quantityType);

        EntityState stateAfterInsert = this._ctx.Entry(quantityType).State;
        int idAfterInsert = quantityType.Id;
        DateTime createdAtAfterInsert = quantityType.CreatedAt;
        DateTime updatedAtAfterInsert = quantityType.UpdatedAt;

        await this._ctx.SaveChangesAsync(false, TestContext.Current.CancellationToken);

        EntityState stateAfterSaveChanges = this._ctx.Entry(quantityType).State;
        int idAfterSaveChanges = quantityType.Id;
        DateTime createdAtAfterSaveChanges = quantityType.CreatedAt;
        DateTime updatedAtAfterSaveChanges = quantityType.UpdatedAt;

        // Assert                      
        Assert.Equal(EntityState.Detached, stateBeforeInsert);
        Assert.Equal(0, idBeforeInsert);
        Assert.Equal(DateTime.MinValue, createdAtBeforeInsert);
        Assert.Equal(DateTime.MinValue, updatedAtBeforeInsert);

        Assert.Equal(EntityState.Added, stateAfterInsert);
        Assert.Equal(0, idAfterInsert);
        Assert.Equal(DateTime.MinValue, createdAtAfterInsert);
        Assert.Equal(DateTime.MinValue, updatedAtAfterInsert);

        Assert.Equal(EntityState.Added, stateAfterSaveChanges);
        Assert.Equal(0, idAfterSaveChanges);
        Assert.NotEqual(DateTime.MinValue, createdAtAfterSaveChanges);
        Assert.NotEqual(DateTime.MinValue, updatedAtAfterSaveChanges);
    }

    [Fact]
    public void SaveChanges_WithFalse_OnUpdate_SetsUpdatedAtButKeepsStateAsModified()
    {
        // Arrange                                 
        Pizza pizza = GetTestPizza("Gorgonzola", null, 12.50m);
        this._ctx.Pizzas.Add(pizza);
        this._ctx.SaveChanges();

        EntityState stateBeforeUpdate = this._ctx.Entry(pizza).State;
        int idBeforeUpdate = pizza.Id;
        DateTime createdAtBeforeUpdate = pizza.CreatedAt;
        DateTime updatedAtBeforeUpdate = pizza.UpdatedAt;

        // Act                                                
        pizza.UpdateDescription(new PizzaDescription("A pizza description."));

        EntityState stateAfterUpdate = this._ctx.Entry(pizza).State;
        int idAfterUpdate = pizza.Id;
        DateTime createdAtAfterUpdate = pizza.CreatedAt;
        DateTime updatedAtAfterUpdate = pizza.UpdatedAt;

        this._ctx.SaveChanges(false);

        EntityState stateAfterSaveChanges = this._ctx.Entry(pizza).State;
        int idAfterSaveChanges = pizza.Id;
        DateTime createdAtAfterSaveChanges = pizza.CreatedAt;
        DateTime updatedAtAfterSaveChanges = pizza.UpdatedAt;

        // Assert
        Assert.Equal(EntityState.Unchanged, stateBeforeUpdate);
        Assert.Equal(1, idBeforeUpdate);
        Assert.NotEqual(DateTime.MinValue, createdAtBeforeUpdate);
        Assert.NotEqual(DateTime.MinValue, updatedAtBeforeUpdate);

        Assert.Equal(EntityState.Modified, stateAfterUpdate);
        Assert.Equal(idBeforeUpdate, idAfterUpdate);
        Assert.Equal(createdAtBeforeUpdate, createdAtAfterUpdate);
        Assert.Equal(updatedAtBeforeUpdate, updatedAtAfterUpdate);

        Assert.Equal(EntityState.Modified, stateAfterSaveChanges);
        Assert.Equal(idAfterUpdate, idAfterSaveChanges);
        Assert.Equal(createdAtAfterUpdate, createdAtAfterSaveChanges);
        Assert.NotEqual(updatedAtAfterUpdate, updatedAtAfterSaveChanges);
    }

    [Fact]
    public async Task SaveChangesAsync_WithFalse_OnUpdate_SetsUpdatedAtButKeepsStateAsModified()
    {
        // Arrange                                  
        Pizza pizza = GetTestPizza("Gorgonzola", null, 12.50m);
        this._ctx.Pizzas.Add(pizza);
        await this._ctx.SaveChangesAsync(TestContext.Current.CancellationToken);

        EntityState stateBeforeUpdate = this._ctx.Entry(pizza).State;
        int idBeforeUpdate = pizza.Id;
        DateTime createdAtBeforeUpdate = pizza.CreatedAt;
        DateTime updatedAtBeforeUpdate = pizza.UpdatedAt;

        // Act                                                
        pizza.UpdateDescription(new PizzaDescription("A pizza description."));

        EntityState stateAfterUpdate = this._ctx.Entry(pizza).State;
        int idAfterUpdate = pizza.Id;
        DateTime createdAtAfterUpdate = pizza.CreatedAt;
        DateTime updatedAtAfterUpdate = pizza.UpdatedAt;

        await this._ctx.SaveChangesAsync(false, TestContext.Current.CancellationToken);

        EntityState stateAfterSaveChanges = this._ctx.Entry(pizza).State;
        int idAfterSaveChanges = pizza.Id;
        DateTime createdAtAfterSaveChanges = pizza.CreatedAt;
        DateTime updatedAtAfterSaveChanges = pizza.UpdatedAt;

        // Assert
        Assert.Equal(EntityState.Unchanged, stateBeforeUpdate);
        Assert.Equal(1, idBeforeUpdate);
        Assert.NotEqual(DateTime.MinValue, createdAtBeforeUpdate);
        Assert.NotEqual(DateTime.MinValue, updatedAtBeforeUpdate);

        Assert.Equal(EntityState.Modified, stateAfterUpdate);
        Assert.Equal(idBeforeUpdate, idAfterUpdate);
        Assert.Equal(createdAtBeforeUpdate, createdAtAfterUpdate);
        Assert.Equal(updatedAtBeforeUpdate, updatedAtAfterUpdate);

        Assert.Equal(EntityState.Modified, stateAfterSaveChanges);
        Assert.Equal(idAfterUpdate, idAfterSaveChanges);
        Assert.Equal(createdAtAfterUpdate, createdAtAfterSaveChanges);
        Assert.NotEqual(updatedAtAfterUpdate, updatedAtAfterSaveChanges);
    }

    [Fact]
    public void SaveChanges_WithTrue_OnInsert_SameAsWithoutParameters()
    {
        // Arrange                                 
        QuantityType quantityType = GetTestQuantityType("Gram", "g");

        EntityState stateBeforeInsert = this._ctx.Entry(quantityType).State;
        int idBeforeInsert = quantityType.Id;
        DateTime createdAtBeforeInsert = quantityType.CreatedAt;
        DateTime updatedAtBeforeInsert = quantityType.UpdatedAt;

        // Act                            
        this._ctx.QuantityTypes.Add(quantityType);

        EntityState stateAfterInsert = this._ctx.Entry(quantityType).State;
        int idAfterInsert = quantityType.Id;
        DateTime createdAtAfterInsert = quantityType.CreatedAt;
        DateTime updatedAtAfterInsert = quantityType.UpdatedAt;

        this._ctx.SaveChanges(true);

        EntityState stateAfterSaveChanges = this._ctx.Entry(quantityType).State;
        int idAfterSaveChanges = quantityType.Id;
        DateTime createdAtAfterSaveChanges = quantityType.CreatedAt;
        DateTime updatedAtAfterSaveChanges = quantityType.UpdatedAt;

        // Assert
        Assert.Equal(EntityState.Detached, stateBeforeInsert);
        Assert.Equal(0, idBeforeInsert);
        Assert.Equal(DateTime.MinValue, createdAtBeforeInsert);
        Assert.Equal(DateTime.MinValue, updatedAtBeforeInsert);

        Assert.Equal(EntityState.Added, stateAfterInsert);
        Assert.Equal(0, idAfterInsert);
        Assert.Equal(DateTime.MinValue, createdAtAfterInsert);
        Assert.Equal(DateTime.MinValue, updatedAtAfterInsert);

        Assert.Equal(EntityState.Unchanged, stateAfterSaveChanges);
        Assert.Equal(1, idAfterSaveChanges);
        Assert.NotEqual(DateTime.MinValue, createdAtAfterSaveChanges);
        Assert.NotEqual(DateTime.MinValue, updatedAtAfterSaveChanges);
    }

    [Fact]
    public async Task SaveChangesAsync_WithTrue_OnInsert_SameAsWithoutParameters()
    {
        // Arrange                                
        QuantityType quantityType = GetTestQuantityType("Gram", "g");

        EntityState stateBeforeInsert = this._ctx.Entry(quantityType).State;
        int idBeforeInsert = quantityType.Id;
        DateTime createdAtBeforeInsert = quantityType.CreatedAt;
        DateTime updatedAtBeforeInsert = quantityType.UpdatedAt;

        // Act                            
        this._ctx.QuantityTypes.Add(quantityType);

        EntityState stateAfterInsert = this._ctx.Entry(quantityType).State;
        int idAfterInsert = quantityType.Id;
        DateTime createdAtAfterInsert = quantityType.CreatedAt;
        DateTime updatedAtAfterInsert = quantityType.UpdatedAt;

        await this._ctx.SaveChangesAsync(true, TestContext.Current.CancellationToken);

        EntityState stateAfterSaveChanges = this._ctx.Entry(quantityType).State;
        int idAfterSaveChanges = quantityType.Id;
        DateTime createdAtAfterSaveChanges = quantityType.CreatedAt;
        DateTime updatedAtAfterSaveChanges = quantityType.UpdatedAt;

        // Assert              
        Assert.Equal(EntityState.Detached, stateBeforeInsert);
        Assert.Equal(0, idBeforeInsert);
        Assert.Equal(DateTime.MinValue, createdAtBeforeInsert);
        Assert.Equal(DateTime.MinValue, updatedAtBeforeInsert);

        Assert.Equal(EntityState.Added, stateAfterInsert);
        Assert.Equal(0, idAfterInsert);
        Assert.Equal(DateTime.MinValue, createdAtAfterInsert);
        Assert.Equal(DateTime.MinValue, updatedAtAfterInsert);

        Assert.Equal(EntityState.Unchanged, stateAfterSaveChanges);
        Assert.Equal(1, idAfterSaveChanges);
        Assert.NotEqual(DateTime.MinValue, createdAtAfterSaveChanges);
        Assert.NotEqual(DateTime.MinValue, updatedAtAfterSaveChanges);
    }

    [Fact]
    public void SaveChanges_WithTrue_OnUpdate_SameAsWithoutParameters()
    {
        // Arrange
        Pizza pizza = GetTestPizza("Gorgonzola", null, 12.50m);
        this._ctx.Pizzas.Add(pizza);
        this._ctx.SaveChanges();

        EntityState stateBeforeUpdate = this._ctx.Entry(pizza).State;
        int idBeforeUpdate = pizza.Id;
        DateTime createdAtBeforeUpdate = pizza.CreatedAt;
        DateTime updatedAtBeforeUpdate = pizza.UpdatedAt;

        // Act
        pizza.UpdateDescription(new PizzaDescription("A pizza description."));

        EntityState stateAfterUpdate = this._ctx.Entry(pizza).State;
        int idAfterUpdate = pizza.Id;
        DateTime createdAtAfterUpdate = pizza.CreatedAt;
        DateTime updatedAtAfterUpdate = pizza.UpdatedAt;

        this._ctx.SaveChanges(true);

        EntityState stateAfterSaveChanges = this._ctx.Entry(pizza).State;
        int idAfterSaveChanges = pizza.Id;
        DateTime createdAtAfterSaveChanges = pizza.CreatedAt;
        DateTime updatedAtAfterSaveChanges = pizza.UpdatedAt;

        // Assert
        Assert.Equal(EntityState.Unchanged, stateBeforeUpdate);
        Assert.Equal(1, idBeforeUpdate);
        Assert.NotEqual(DateTime.MinValue, createdAtBeforeUpdate);
        Assert.NotEqual(DateTime.MinValue, updatedAtBeforeUpdate);

        Assert.Equal(EntityState.Modified, stateAfterUpdate);
        Assert.Equal(idBeforeUpdate, idAfterUpdate);
        Assert.Equal(createdAtBeforeUpdate, createdAtAfterUpdate);
        Assert.Equal(updatedAtBeforeUpdate, updatedAtAfterUpdate);

        Assert.Equal(EntityState.Unchanged, stateAfterSaveChanges);
        Assert.Equal(idAfterUpdate, idAfterSaveChanges);
        Assert.Equal(createdAtAfterUpdate, createdAtAfterSaveChanges);
        Assert.NotEqual(updatedAtAfterUpdate, updatedAtAfterSaveChanges);
    }

    [Fact]
    public async Task SaveChangesAsync_WithTrue_OnUpdate_SameAsWithoutParameters()
    {
        // Arrange
        Pizza pizza = GetTestPizza("Gorgonzola", null, 12.50m);
        this._ctx.Pizzas.Add(pizza);
        await this._ctx.SaveChangesAsync(TestContext.Current.CancellationToken);

        EntityState stateBeforeUpdate = this._ctx.Entry(pizza).State;
        int idBeforeUpdate = pizza.Id;
        DateTime createdAtBeforeUpdate = pizza.CreatedAt;
        DateTime updatedAtBeforeUpdate = pizza.UpdatedAt;

        // Act
        pizza.UpdateDescription(new PizzaDescription("A pizza description."));

        EntityState stateAfterUpdate = this._ctx.Entry(pizza).State;
        int idAfterUpdate = pizza.Id;
        DateTime createdAtAfterUpdate = pizza.CreatedAt;
        DateTime updatedAtAfterUpdate = pizza.UpdatedAt;

        await this._ctx.SaveChangesAsync(true, TestContext.Current.CancellationToken);

        EntityState stateAfterSaveChanges = this._ctx.Entry(pizza).State;
        int idAfterSaveChanges = pizza.Id;
        DateTime createdAtAfterSaveChanges = pizza.CreatedAt;
        DateTime updatedAtAfterSaveChanges = pizza.UpdatedAt;

        // Assert
        Assert.Equal(EntityState.Unchanged, stateBeforeUpdate);
        Assert.Equal(1, idBeforeUpdate);
        Assert.NotEqual(DateTime.MinValue, createdAtBeforeUpdate);
        Assert.NotEqual(DateTime.MinValue, updatedAtBeforeUpdate);

        Assert.Equal(EntityState.Modified, stateAfterUpdate);
        Assert.Equal(idBeforeUpdate, idAfterUpdate);
        Assert.Equal(createdAtBeforeUpdate, createdAtAfterUpdate);
        Assert.Equal(updatedAtBeforeUpdate, updatedAtAfterUpdate);

        Assert.Equal(EntityState.Unchanged, stateAfterSaveChanges);
        Assert.Equal(idAfterUpdate, idAfterSaveChanges);
        Assert.Equal(createdAtAfterUpdate, createdAtAfterSaveChanges);
        Assert.NotEqual(updatedAtAfterUpdate, updatedAtAfterSaveChanges);
    }
}