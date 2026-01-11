namespace IlVecchioForno.Domain.Commons.Exceptions;

public abstract class ValueObjectBaseException : ArgumentException
{
    protected ValueObjectBaseException(string message) : base(message)
    {
    }
}