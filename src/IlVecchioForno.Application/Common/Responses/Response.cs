namespace IlVecchioForno.Application.Common.Responses;

public sealed record Response<T>(
    ResponseType Type,
    T Content
) : IResponse;