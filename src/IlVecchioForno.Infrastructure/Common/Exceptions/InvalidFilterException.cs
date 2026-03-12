namespace IlVecchioForno.Infrastructure.Common.Exceptions;

public class InvalidFilterException : Exception
{
    public InvalidFilterException(string? entityName, Exception innerException)
        : base($"Provided filter is not supported for entity {entityName}.", innerException)
    {
    }
}