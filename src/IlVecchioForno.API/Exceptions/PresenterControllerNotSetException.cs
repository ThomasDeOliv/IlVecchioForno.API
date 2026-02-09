namespace IlVecchioForno.API.Exceptions;

public class PresenterControllerNotSetException : InvalidOperationException
{
    public PresenterControllerNotSetException()
        : base(
            "No controller was set on the presenter. Ensure the controller calls Initialize before using the presenter."
        )
    {
    }
}