namespace IlVecchioForno.Application.Common.Exceptions;

public sealed class InvalidReferenceException : Exception
{
    public InvalidReferenceException(string message) : base(message)
    {
    }
}