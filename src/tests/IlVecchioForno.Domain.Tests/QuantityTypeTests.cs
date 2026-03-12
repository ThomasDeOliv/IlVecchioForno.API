using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Domain.QuantityTypes.Exceptions;

namespace IlVecchioForno.Domain.Tests;

public class QuantityTypeTests
{
    [Fact]
    public void QuantityType_CreateInstance_Succeeds_WhenProvidingNullUnit()
    {
        // Arrange
        const string quantityTypeNameBase = "Grams";
        const string quantityTypeUnitBase = "g";
        QuantityTypeName quantityTypeName = new QuantityTypeName(quantityTypeNameBase);
        QuantityTypeUnit quantityTypeUnit = new QuantityTypeUnit(quantityTypeUnitBase);
        // Act
        QuantityType quantityType = new QuantityType(quantityTypeName, quantityTypeUnit);
        // Assert
        Assert.Equal(quantityTypeNameBase, quantityType.Name);
        Assert.Equal(quantityTypeUnitBase, quantityType.Unit);
    }

    [Theory]
    [InlineData("Grams", "g")]
    [InlineData("Kilograms", "kg")]
    [InlineData("Milliliters", "mL")]
    [InlineData("Liters", "L")]
    public void QuantityType_CreateInstance_Succeeds_WhenProvidingValidNameAndUnit(string name, string unit)
    {
        // Arrange
        QuantityTypeName nameValueObject = new QuantityTypeName(name);
        QuantityTypeUnit unitValueObject = new QuantityTypeUnit(unit);
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
        QuantityTypeNameException
            exception = Assert.Throws<QuantityTypeNameException>(() => new QuantityTypeName(name));
        // Assert
        Assert.Equal(
            $"{nameof(QuantityTypeName)} exceeds maximum length of {QuantityTypeInvariant.NameMaxLength} characters.",
            exception.Message
        );
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
}