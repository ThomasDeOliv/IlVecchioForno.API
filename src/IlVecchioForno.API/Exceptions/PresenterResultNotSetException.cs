namespace IlVecchioForno.API.Exceptions;

public sealed class PresenterResultNotSetException : InvalidOperationException
{
    public PresenterResultNotSetException()
        : base("No result was set by the presenter. Ensure the handler calls a presenter method.")
    {
    }
}