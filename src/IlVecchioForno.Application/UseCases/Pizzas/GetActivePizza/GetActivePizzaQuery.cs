using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.GetActivePizza;

public sealed record GetActivePizzaQuery(
    int Id
) : IRequest<ActivePizzaDto?>;