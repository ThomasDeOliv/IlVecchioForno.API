namespace IlVecchioForno.Application.Common.Responses;

public sealed record ErrorResponseWithMessage(
    ErrorResponseType Type,
    string Message
) : IResponse;