namespace IlVecchioForno.Application.Common.Exceptions;

public sealed class MappingException : InvalidOperationException
{
    public MappingException(string message) : base(message)
    {
    }
}