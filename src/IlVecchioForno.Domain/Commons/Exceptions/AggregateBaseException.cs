namespace IlVecchioForno.Domain.Commons.Exceptions;

public abstract class AggregateBaseException : ArgumentException
{
    protected AggregateBaseException(string message) : base(message)
    {
    }
}