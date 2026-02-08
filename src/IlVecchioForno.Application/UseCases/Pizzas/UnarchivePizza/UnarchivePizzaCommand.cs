using IlVecchioForno.Application.Common.Responses;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.UnarchivePizza;

public sealed record UnarchivePizzaCommand(
    int Id
) : IRequest<IResponse>;