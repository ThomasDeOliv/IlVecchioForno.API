namespace IlVecchioForno.API.Exceptions;

public class PresenterControllerNameNotSetException : InvalidOperationException
{
    public PresenterControllerNameNotSetException()
        : base("No controller name was set on the presenter. Ensure the controller calls Initialize before using the presenter.")
    {
    }
}