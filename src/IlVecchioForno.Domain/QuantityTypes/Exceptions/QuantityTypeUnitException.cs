using IlVecchioForno.Domain.Common.Exceptions;

namespace IlVecchioForno.Domain.QuantityTypes.Exceptions;

public sealed class QuantityTypeUnitException : ValueObjectBaseException
{
    public QuantityTypeUnitException(string message) : base(message)
    {
    }
}