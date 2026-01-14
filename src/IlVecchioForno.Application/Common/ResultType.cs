namespace IlVecchioForno.Application.Common;

public enum ResultType
{
    Ok,
    Created,
    Conflict,
    ValidationError,
    Unauthorized,
    Forbidden,
    NotFound,
    InternalError
}