namespace IlVecchioForno.API.Exceptions;

public sealed class PresenterInvalidControllerProvidedException : InvalidOperationException
{
    public PresenterInvalidControllerProvidedException(Type expected, Type actual) : base(
        $"Expected controller of type '{expected.Name}' but received '{actual.Name}'."
    )
    {
    }
}