namespace IlVecchioForno.Application.Common;

public sealed class Result<T>
{
    private Result()
    {
    }

    public bool Success { get; private init; }
    public string? ErrorMessage { get; private init; }
    public ResultType Type { get; private init; }
    public T? Content { get; private init; }

    public static Result<T> Ok(T content)
    {
        return new Result<T>
        {
            Success = true,
            Content = content,
            Type = ResultType.Ok,
            ErrorMessage = null
        };
    }
    
    public static Result<T> Created(T content)
    {
        return new Result<T>
        {
            Success = true,
            Content = content,
            Type = ResultType.Created,
            ErrorMessage = null
        };
    }

    public static Result<T> NotFound(string errorMessage)
    {
        return new Result<T>
        {
            Success = false,
            Content = default,
            Type = ResultType.NotFound,
            ErrorMessage = errorMessage
        };
    }

    public static Result<T> ValidationError(string errorMessage)
    {
        return new Result<T>
        {
            Success = false,
            Content = default,
            Type = ResultType.ValidationError,
            ErrorMessage = errorMessage
        };
    }

    public static Result<T> Conflict(string errorMessage)
    {
        return new Result<T>
        {
            Success = false,
            Content = default,
            Type = ResultType.Conflict,
            ErrorMessage = errorMessage
        };
    }

    public static Result<T> Unauthorized(string errorMessage)
    {
        return new Result<T>
        {
            Success = false,
            Content = default,
            Type = ResultType.Unauthorized,
            ErrorMessage = errorMessage
        };
    }

    public static Result<T> Forbidden(string errorMessage)
    {
        return new Result<T>
        {
            Success = false,
            Content = default,
            Type = ResultType.Forbidden,
            ErrorMessage = errorMessage
        };
    }
}