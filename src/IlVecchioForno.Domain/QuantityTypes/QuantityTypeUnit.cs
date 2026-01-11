using IlVecchioForno.Domain.QuantityTypes.Exceptions;

namespace IlVecchioForno.Domain.QuantityTypes;

public sealed class QuantityTypeUnit
{
    private readonly string _value;
    
    public QuantityTypeUnit(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new QuantityTypeUnitException("Unit cannot be composed of whitespace characters only.");
        }

        if (QuantityTypeInvariant.UnitMaxLength < value.Length)
        {
            throw new QuantityTypeUnitException($"Unit maximum length reached ({QuantityTypeInvariant.UnitMaxLength} characters).");
        }
        
        this._value = value;
    }
    
    public static implicit operator string(QuantityTypeUnit valueObject) =>  valueObject._value;
}