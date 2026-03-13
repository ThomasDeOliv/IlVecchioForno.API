using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.PizzaIngredients;
using IlVecchioForno.Domain.PizzaIngredients.Exceptions;
using IlVecchioForno.Domain.QuantityTypes;

namespace IlVecchioForno.Domain.Tests;

public class PizzaIngredientTests
{
    public static TheoryData<decimal> ValidQuantities =>
        new TheoryData<decimal>
        {
            0m,
            1m,
            2m,
            0.5m,
            0.008m,
            100m,
            250m,
            1000m,
            8000m
        };

    public static TheoryData<decimal> InvalidQuantities =>
        new TheoryData<decimal>
        {
            -1m,
            -2m,
            -0.5m,
            -0.008m,
            -100m,
            -250m,
            -1000m,
            -8000m
        };

    [Theory]
    [MemberData(nameof(ValidQuantities))]
    public void PizzaIngredient_CreateInstance_Succeeds_WhenProvidingValidValue(decimal quantity)
    {
        // Arrange
        const string quantityTypeNameBase = "Grams";
        const string quantityTypeUnitBase = "g";
        const string ingredientNameBase = "Basil";
        QuantityTypeName quantityTypeName = quantityTypeNameBase;
        QuantityTypeUnit quantityTypeUnit = quantityTypeUnitBase;
        QuantityType quantityType = new QuantityType(quantityTypeName, quantityTypeUnit);
        IngredientName ingredientName = ingredientNameBase;
        Ingredient ingredient = new Ingredient(ingredientName, quantityType);
        PizzaIngredientQuantity pizzaIngredientQuantity = quantity;
        // Act
        PizzaIngredient pizzaIngredient = new PizzaIngredient(pizzaIngredientQuantity, ingredient);
        // Assert
        Assert.Equal(quantity, (decimal)pizzaIngredient.Quantity);
    }

    [Theory]
    [MemberData(nameof(InvalidQuantities))]
    public void PizzaIngredientQuantity_CreateInstance_ThrowsException_WhenProvidingInvalidValue(
        decimal quantity)
    {
        // Arrange & Act
        PizzaIngredientQuantityException exception = Assert.Throws<PizzaIngredientQuantityException>(() => new PizzaIngredientQuantity(quantity));
        // Assert
        Assert.Equal(
            $"{nameof(PizzaIngredientQuantity)} is smaller than the minimum required value of {PizzaIngredientInvariant.MinQuantity}.",
            exception.Message
        );
    }

    [Fact]
    public void PizzaIngredientQuantity_CanBeImplicitlyCastToDecimal()
    {
        // Arrange
        const decimal rawValue = 15.0m;
        PizzaIngredientQuantity valueObject = new PizzaIngredientQuantity(rawValue);
        // Act
        decimal result = valueObject;
        // Assert
        Assert.IsType<decimal>(result);
        Assert.Equal(rawValue, result);
    }

    [Fact]
    public void PizzaIngredientQuantity_CanBeImplicitlyCreatedFromDecimal()
    {
        // Arrange
        const decimal rawValue = 15.0m;
        // Act
        PizzaIngredientQuantity result = rawValue;
        // Assert
        Assert.IsType<PizzaIngredientQuantity>(result);
        Assert.Equal(rawValue, result.Value);
    }

    [Fact]
    public void PizzaIngredientQuantity_CannotBeImplicitlyCreatedFromDecimalBelowMinQuantity()
    {
        // Arrange
        const decimal rawValue = -15.0m;
        // Act & Assert
        PizzaIngredientQuantityException ex = Assert.Throws<PizzaIngredientQuantityException>(() =>
        {
            PizzaIngredientQuantity _ = rawValue;
        });
        Assert.Equal($"{nameof(PizzaIngredientQuantity)} is smaller than the minimum required value of {PizzaIngredientInvariant.MinQuantity}.", ex.Message);
    }
}