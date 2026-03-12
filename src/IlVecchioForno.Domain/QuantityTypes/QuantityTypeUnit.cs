using IlVecchioForno.Domain.QuantityTypes.Exceptions;

namespace IlVecchioForno.Domain.QuantityTypes;

public sealed class QuantityTypeUnit
{
    public QuantityTypeUnit(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new QuantityTypeUnitException(
                $"{nameof(QuantityTypeUnit)} cannot be instantiated from null, empty or whitespace value."
            );

        if (QuantityTypeInvariant.UnitMaxLength < value.Length)
            throw new QuantityTypeUnitException(
                $"{nameof(QuantityTypeUnit)} exceeds maximum length of {QuantityTypeInvariant.UnitMaxLength} characters."
            );

        this.Value = value;
    }

    public string Value { get; }

    public static implicit operator string(QuantityTypeUnit valueObject)
    {
        return valueObject.Value;
    }

    public static implicit operator QuantityTypeUnit(string value)
    {
        return new QuantityTypeUnit(value);
    }
}