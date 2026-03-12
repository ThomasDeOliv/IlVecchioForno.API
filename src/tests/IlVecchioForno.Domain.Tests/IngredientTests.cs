using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.Ingredients.Exceptions;
using IlVecchioForno.Domain.QuantityTypes;

namespace IlVecchioForno.Domain.Tests;

public class IngredientTests
{
    [Theory]
    [InlineData("Basil")]
    [InlineData("Tomato sauce")]
    [InlineData("Cream")]
    [InlineData("Gorgonzola")]
    [InlineData("Parmigiano Regiano")]
    public void Ingredient_CreateInstance_Succeeds_WhenProvidingValidValue(string name)
    {
        // Arrange
        IngredientName ingredientName = new IngredientName(name);
        const string quantityTypeNameBase = "Grams";
        const string? quantityTypeUnitBase = "g";
        QuantityTypeName quantityTypeName = new QuantityTypeName(quantityTypeNameBase);
        QuantityTypeUnit quantityTypeUnit = new QuantityTypeUnit(quantityTypeUnitBase);
        QuantityType quantityType = new QuantityType(quantityTypeName, quantityTypeUnit);
        // Arrange & Act
        Ingredient ingredient = new Ingredient(ingredientName, quantityType);
        // Assert
        Assert.Equal(name, ingredient.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("     ")]
    [InlineData("                          ")]
    public void IngredientName_CreateInstance_ThrowsException_WhenProvidingInvalidValue(string? name)
    {
        // Arrange & Act
        IngredientNameException exception = Assert.Throws<IngredientNameException>(() => new IngredientName(name!));
        // Assert
        Assert.Equal(
            $"{nameof(IngredientName)} cannot be instantiated from null, empty or whitespace value.",
            exception.Message
        );
    }


    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(1000)]
    public void IngredientName_CreateInstance_ThrowsException_WhenExceedingMaxLength(int length)
    {
        // Arrange
        string name = new string('a', IngredientInvariant.NameMaxLength + length);
        // Act
        IngredientNameException exception = Assert.Throws<IngredientNameException>(() => new IngredientName(name));
        // Assert
        Assert.Equal(
            $"{nameof(IngredientName)} exceeds maximum length of {IngredientInvariant.NameMaxLength} characters.",
            exception.Message
        );
    }
}