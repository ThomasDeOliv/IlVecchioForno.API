namespace IlVecchioForno.Application.Common.Responses;

public sealed record ResponseWithErrorMessages(
    Dictionary<string, string[]> Messages
) : IResponse;