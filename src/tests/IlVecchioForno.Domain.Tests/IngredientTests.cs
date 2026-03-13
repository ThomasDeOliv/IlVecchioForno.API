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
        QuantityType quantityType = new QuantityType(quantityTypeNameBase, quantityTypeUnitBase);
        // Arrange & Act
        Ingredient ingredient = new Ingredient(ingredientName, quantityType);
        // Assert
        Assert.Equal(name, ingredient.Name);
    }

    [Theory]
    [InlineData("Fresh basil leaves")]
    [InlineData("Garlic clove")]
    [InlineData("Black olives")]
    public void Ingredient_CreateInstance_Succeeds_WhenQuantityTypeIsNull(string name)
    {
        // Arrange & Act
        Ingredient ingredient = new Ingredient(name, null);
        // Assert
        Assert.Equal(name, ingredient.Name);
        Assert.Null(ingredient.QuantityType);
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

    [Fact]
    public void IngredientName_CanBeImplicitlyCastToString()
    {
        // Arrange
        const string rawValue = "A random name...";
        IngredientName valueObject = new IngredientName(rawValue);
        // Act
        string result = valueObject;
        // Assert
        Assert.IsType<string>(result);
        Assert.Equal(rawValue, result);
    }

    [Fact]
    public void IngredientName_CanBeImplicitlyCreatedFromValidString()
    {
        // Arrange
        const string rawValue = "A random name...";
        // Act
        IngredientName result = rawValue;
        // Assert
        Assert.IsType<IngredientName>(result);
        Assert.Equal(rawValue, result.Value);
    }

    [Fact]
    public void IngredientName_CannotBeImplicitlyCreatedFromNull()
    {
        // Arrange
        const string? rawValue = null;
        // Act & Assert
        IngredientNameException ex = Assert.Throws<IngredientNameException>(() =>
        {
            IngredientName _ = rawValue!;
        });
        Assert.Equal($"{nameof(IngredientName)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void IngredientName_CannotBeImplicitlyCreatedFromEmptyString()
    {
        // Arrange
        const string rawValue = "";
        // Act & Assert
        IngredientNameException ex = Assert.Throws<IngredientNameException>(() =>
        {
            IngredientName _ = rawValue!;
        });
        Assert.Equal($"{nameof(IngredientName)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void IngredientName_CannotBeImplicitlyCreatedFromStringFilledWithWhiteSpaces()
    {
        // Arrange
        const string rawValue = "                  ";
        // Act & Assert
        IngredientNameException ex = Assert.Throws<IngredientNameException>(() =>
        {
            IngredientName _ = rawValue!;
        });
        Assert.Equal($"{nameof(IngredientName)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void IngredientName_CannotBeImplicitlyCreatedFromStringExceedingMaxLength()
    {
        // Arrange
        string rawValue = new string('c', IngredientInvariant.NameMaxLength + 1);
        // Act & Assert
        IngredientNameException ex = Assert.Throws<IngredientNameException>(() =>
        {
            IngredientName _ = rawValue!;
        });
        Assert.Equal($"{nameof(IngredientName)} exceeds maximum length of {IngredientInvariant.NameMaxLength} characters.", ex.Message);
    }
}