using IlVecchioForno.Application.Common.Responses;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.GetActivePizza;

public sealed record GetActivePizzaQuery(
    int Id
) : IRequest<IResponse>;