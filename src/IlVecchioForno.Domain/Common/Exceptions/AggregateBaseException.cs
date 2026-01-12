namespace IlVecchioForno.Domain.Common.Exceptions;

public abstract class AggregateBaseException : ArgumentException
{
    protected AggregateBaseException(string message) : base(message)
    {
    }
}