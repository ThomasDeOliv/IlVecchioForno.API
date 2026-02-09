namespace IlVecchioForno.Application.Common.Responses;

public sealed record ErrorResponseWithMessages(
    Dictionary<string, string[]> Messages
) : IResponse;