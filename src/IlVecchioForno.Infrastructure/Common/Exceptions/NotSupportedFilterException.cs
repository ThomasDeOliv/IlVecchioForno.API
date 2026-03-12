namespace IlVecchioForno.Infrastructure.Common.Exceptions;

public class NotSupportedFilterException : Exception
{
    public NotSupportedFilterException(string? entityName, Exception innerException)
        : base($"Provided filter is not supported for entity {entityName}.", innerException)
    {
    }
}