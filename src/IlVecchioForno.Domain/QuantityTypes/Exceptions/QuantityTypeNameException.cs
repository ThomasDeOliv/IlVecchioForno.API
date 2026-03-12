using IlVecchioForno.Domain.Common.Exceptions;

namespace IlVecchioForno.Domain.QuantityTypes.Exceptions;

public sealed class QuantityTypeNameException : ValueObjectBaseException
{
    public QuantityTypeNameException(string message) : base(message)
    {
    }
}