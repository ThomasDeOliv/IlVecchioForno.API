namespace IlVecchioForno.Application.Common.Responses;

public sealed record ResponseWithErrorMessage(
    ErrorMessageType Type,
    string Message
) : IResponse;