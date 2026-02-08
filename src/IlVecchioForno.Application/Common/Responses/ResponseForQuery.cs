namespace IlVecchioForno.Application.Common.Responses;

public sealed record ResponseForQuery<T>(
    T Content
) : IResponse;