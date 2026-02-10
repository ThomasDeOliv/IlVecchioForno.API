namespace IlVecchioForno.API.Exceptions;

public class PresenterNotInitializedException : InvalidOperationException
{
    public PresenterNotInitializedException()
        : base(
            "No controller was set on the presenter. Ensure the controller calls Initialize before using the presenter."
        )
    {
    }
}