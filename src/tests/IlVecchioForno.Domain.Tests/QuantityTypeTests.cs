using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Domain.QuantityTypes.Exceptions;

namespace IlVecchioForno.Domain.Tests;

public class QuantityTypeTests
{
    [Theory]
    [InlineData("Grams", "g")]
    [InlineData("Kilograms", "kg")]
    [InlineData("Milliliters", "mL")]
    [InlineData("Liters", "L")]
    public void QuantityType_CreateInstance_Succeeds_WhenProvidingValidNameAndUnit(string name, string unit)
    {
        // Arrange
        QuantityTypeName nameValueObject = name;
        QuantityTypeUnit unitValueObject = unit;
        // Act
        QuantityType quantityType = new QuantityType(nameValueObject, unitValueObject);
        // Assert
        Assert.Equal(name, quantityType.Name);
        Assert.NotNull(quantityType.Unit);
        Assert.Equal(unit, quantityType.Unit);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("     ")]
    [InlineData("                          ")]
    public void QuantityTypeName_CreateInstance_ThrowsException_WhenProvidingInvalidValue(string? name)
    {
        // Arrange & Act
        QuantityTypeNameException exception =
            Assert.Throws<QuantityTypeNameException>(() => new QuantityTypeName(name!));
        // Assert
        Assert.Equal(
            $"{nameof(QuantityTypeName)} cannot be instantiated from null, empty or whitespace value.",
            exception.Message
        );
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(1000)]
    public void QuantityTypeName_CreateInstance_ThrowsException_WhenExceedingMaxLength(int length)
    {
        // Arrange
        string name = new string('a', QuantityTypeInvariant.NameMaxLength + length);
        // Act
        QuantityTypeNameException exception = Assert.Throws<QuantityTypeNameException>(() => new QuantityTypeName(name));
        // Assert
        Assert.Equal(
            $"{nameof(QuantityTypeName)} exceeds maximum length of {QuantityTypeInvariant.NameMaxLength} characters.",
            exception.Message
        );
    }

    [Fact]
    public void QuantityTypeName_CanBeImplicitlyCastToString()
    {
        // Arrange
        const string rawValue = "A random name...";
        QuantityTypeName valueObject = new QuantityTypeName(rawValue);
        // Act
        string result = valueObject;
        // Assert
        Assert.IsType<string>(result);
        Assert.Equal(rawValue, result);
    }

    [Fact]
    public void QuantityTypeName_CanBeImplicitlyCreatedFromValidString()
    {
        // Arrange
        const string rawValue = "A random name...";
        // Act
        QuantityTypeName result = rawValue;
        // Assert
        Assert.IsType<QuantityTypeName>(result);
        Assert.Equal(rawValue, result.Value);
    }

    [Fact]
    public void QuantityTypeName_CannotBeImplicitlyCreatedFromNull()
    {
        // Arrange
        const string? rawValue = null;
        // Act & Assert
        QuantityTypeNameException ex = Assert.Throws<QuantityTypeNameException>(() =>
        {
            QuantityTypeName _ = rawValue!;
        });
        Assert.Equal($"{nameof(QuantityTypeName)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void QuantityTypeName_CannotBeImplicitlyCreatedFromEmptyString()
    {
        // Arrange
        const string rawValue = "";
        // Act & Assert
        QuantityTypeNameException ex = Assert.Throws<QuantityTypeNameException>(() =>
        {
            QuantityTypeName _ = rawValue;
        });
        Assert.Equal($"{nameof(QuantityTypeName)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void QuantityTypeName_CannotBeImplicitlyCreatedFromStringFilledWithWhiteSpaces()
    {
        // Arrange
        const string rawValue = "                  ";
        // Act & Assert
        QuantityTypeNameException ex = Assert.Throws<QuantityTypeNameException>(() =>
        {
            QuantityTypeName _ = rawValue;
        });
        Assert.Equal($"{nameof(QuantityTypeName)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void QuantityTypeName_CannotBeImplicitlyCreatedFromStringExceedingMaxLength()
    {
        // Arrange
        string rawValue = new string('c', QuantityTypeInvariant.NameMaxLength + 1);
        // Act & Assert
        QuantityTypeNameException ex = Assert.Throws<QuantityTypeNameException>(() =>
        {
            QuantityTypeName _ = rawValue;
        });
        Assert.Equal($"{nameof(QuantityTypeName)} exceeds maximum length of {QuantityTypeInvariant.NameMaxLength} characters.", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("           ")]
    [InlineData("                                  ")]
    public void QuantityTypeUnit_CreateInstance_ThrowsException_WhenProvidingInvalidValue(string? unit)
    {
        // Arrange & Act
        QuantityTypeUnitException exception =
            Assert.Throws<QuantityTypeUnitException>(() => new QuantityTypeUnit(unit!));
        // Assert
        Assert.Equal(
            $"{nameof(QuantityTypeUnit)} cannot be instantiated from null, empty or whitespace value.",
            exception.Message
        );
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(1000)]
    public void QuantityTypeUnit_CreateInstance_ThrowsException_WhenExceedingMaxLength(int length)
    {
        // Arrange
        string unit = new string('a', QuantityTypeInvariant.UnitMaxLength + length);
        // Act
        QuantityTypeUnitException
            exception = Assert.Throws<QuantityTypeUnitException>(() => new QuantityTypeUnit(unit));
        // Assert
        Assert.Equal(
            $"{nameof(QuantityTypeUnit)} exceeds maximum length of {QuantityTypeInvariant.UnitMaxLength} characters.",
            exception.Message
        );
    }

    [Fact]
    public void QuantityTypeUnit_CanBeImplicitlyCastToString()
    {
        // Arrange
        const string rawValue = "ARU"; // A Random Unit
        QuantityTypeUnit valueObject = new QuantityTypeUnit(rawValue);
        // Act
        string result = valueObject;
        // Assert
        Assert.IsType<string>(result);
        Assert.Equal(rawValue, result);
    }

    [Fact]
    public void QuantityTypeUnit_CanBeImplicitlyCreatedFromValidString()
    {
        // Arrange
        const string rawValue = "ARU"; // A Random Unit
        // Act
        QuantityTypeUnit result = rawValue;
        // Assert
        Assert.IsType<QuantityTypeUnit>(result);
        Assert.Equal(rawValue, result.Value);
    }

    [Fact]
    public void QuantityTypeUnit_CannotBeImplicitlyCreatedFromNull()
    {
        // Arrange
        const string? rawValue = null;
        // Act & Assert
        QuantityTypeUnitException ex = Assert.Throws<QuantityTypeUnitException>(() =>
        {
            QuantityTypeUnit _ = rawValue!;
        });
        Assert.Equal($"{nameof(QuantityTypeUnit)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void QuantityTypeUnit_CannotBeImplicitlyCreatedFromEmptyString()
    {
        // Arrange
        const string rawValue = "";
        // Act & Assert
        QuantityTypeUnitException ex = Assert.Throws<QuantityTypeUnitException>(() =>
        {
            QuantityTypeUnit _ = rawValue;
        });
        Assert.Equal($"{nameof(QuantityTypeUnit)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void QuantityTypeUnit_CannotBeImplicitlyCreatedFromStringFilledWithWhiteSpaces()
    {
        // Arrange
        const string rawValue = "                  ";
        // Act & Assert
        QuantityTypeUnitException ex = Assert.Throws<QuantityTypeUnitException>(() =>
        {
            QuantityTypeUnit _ = rawValue;
        });
        Assert.Equal($"{nameof(QuantityTypeUnit)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void QuantityTypeUnit_CannotBeImplicitlyCreatedFromStringExceedingMaxLength()
    {
        // Arrange
        string rawValue = new string('c', QuantityTypeInvariant.UnitMaxLength + 1);
        // Act & Assert
        QuantityTypeUnitException ex = Assert.Throws<QuantityTypeUnitException>(() =>
        {
            QuantityTypeUnit _ = rawValue;
        });
        Assert.Equal($"{nameof(QuantityTypeUnit)} exceeds maximum length of {QuantityTypeInvariant.UnitMaxLength} characters.", ex.Message);
    }
}