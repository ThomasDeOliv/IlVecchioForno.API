using IlVecchioForno.Domain.Commons.Exceptions;

namespace IlVecchioForno.Domain.QuantityTypes.Exceptions;

public sealed class QuantityTypeNameException : ValueObjectBaseException
{
    public QuantityTypeNameException(string message) : base(message)
    {
    }
}