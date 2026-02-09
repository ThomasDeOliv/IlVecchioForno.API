namespace IlVecchioForno.API.Exceptions;

public sealed class PresenterActionNameNotSetException : InvalidOperationException
{
    public PresenterActionNameNotSetException()
        : base("No action name was set on the presenter. Ensure the controller calls Initialize before using the presenter.")
    {
    }
}