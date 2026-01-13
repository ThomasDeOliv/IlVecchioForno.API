namespace IlVecchioForno.Application.Common;

public enum ResultErrorType
{
    None,
    NotFound,
    ValidationError,
    Conflict,
    Unauthorized,
    Forbidden,
    InternalError
}