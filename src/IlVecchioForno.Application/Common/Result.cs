namespace IlVecchioForno.Application.Common;

public sealed class Result<T>
{
    private Result()
    {
    }

    public bool Success { get; private init; }
    public T? Content { get; private init; }
    public ResultErrorType ErrorType { get; private init; }
    public string? ErrorMessage { get; private init; }

    public static Result<T> Ok(T content)
    {
        return new Result<T>
        {
            Success = true,
            Content = content,
            ErrorType = ResultErrorType.None,
            ErrorMessage = null
        };
    }

    public static Result<T> NotFound(string errorMessage)
    {
        return new Result<T>
        {
            Success = false,
            Content = default,
            ErrorType = ResultErrorType.NotFound,
            ErrorMessage = errorMessage
        };
    }

    public static Result<T> ValidationError(string errorMessage)
    {
        return new Result<T>
        {
            Success = false,
            Content = default,
            ErrorType = ResultErrorType.ValidationError,
            ErrorMessage = errorMessage
        };
    }

    public static Result<T> Conflict(string errorMessage)
    {
        return new Result<T>
        {
            Success = false,
            Content = default,
            ErrorType = ResultErrorType.Conflict,
            ErrorMessage = errorMessage
        };
    }

    public static Result<T> Unauthorized(string errorMessage)
    {
        return new Result<T>
        {
            Success = false,
            Content = default,
            ErrorType = ResultErrorType.Unauthorized,
            ErrorMessage = errorMessage
        };
    }

    public static Result<T> Forbidden(string errorMessage)
    {
        return new Result<T>
        {
            Success = false,
            Content = default,
            ErrorType = ResultErrorType.Forbidden,
            ErrorMessage = errorMessage
        };
    }
}