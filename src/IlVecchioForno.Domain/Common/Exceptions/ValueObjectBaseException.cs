namespace IlVecchioForno.Domain.Common.Exceptions;

public abstract class ValueObjectBaseException : ArgumentException
{
    protected ValueObjectBaseException(string message) : base(message)
    {
    }
}