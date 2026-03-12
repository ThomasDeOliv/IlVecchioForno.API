namespace IlVecchioForno.Infrastructure.Common.Exceptions;

public class SaveChangesException : Exception
{
    public SaveChangesException(Exception innerException) : base("SaveChanges failed.", innerException)
    {
    }
}