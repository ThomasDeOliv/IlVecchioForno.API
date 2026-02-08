namespace IlVecchioForno.Application.Common.Responses;

public sealed record ResponseForCommand<T>(
    T Content
) : IResponse;