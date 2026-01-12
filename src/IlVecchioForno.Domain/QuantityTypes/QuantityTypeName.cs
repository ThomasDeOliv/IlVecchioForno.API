using IlVecchioForno.Domain.QuantityTypes.Exceptions;

namespace IlVecchioForno.Domain.QuantityTypes;

public sealed class QuantityTypeName
{
    public QuantityTypeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new QuantityTypeNameException("Name cannot be null, empty or composed of whitespaces.");

        if (QuantityTypeInvariant.NameMaxLength < value.Length)
            throw new QuantityTypeNameException(
                $"Name maximum length reached ({QuantityTypeInvariant.NameMaxLength} characters).");

        this.Value = value;
    }

    public string Value { get; }

    public static implicit operator string(QuantityTypeName valueObject)
    {
        return valueObject.Value;
    }
}