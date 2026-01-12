using IlVecchioForno.Domain.QuantityTypes.Exceptions;

namespace IlVecchioForno.Domain.QuantityTypes;

public sealed class QuantityTypeName
{
    public QuantityTypeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new QuantityTypeNameException(
                $"{nameof(QuantityTypeName)} cannot be instantiated from null, empty or whitespace value."
            );

        if (QuantityTypeInvariant.NameMaxLength < value.Length)
            throw new QuantityTypeNameException(
                $"{nameof(QuantityTypeName)} exceeds maximum length of {QuantityTypeInvariant.NameMaxLength} characters."
            );

        this.Value = value;
    }

    public string Value { get; }

    public static implicit operator string(QuantityTypeName valueObject)
    {
        return valueObject.Value;
    }
}