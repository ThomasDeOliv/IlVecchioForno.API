namespace IlVecchioForno.Application.Common.Exceptions;

public sealed class EntityRegistrationException : Exception
{
    public EntityRegistrationException(string message) : base(message)
    {
    }
}