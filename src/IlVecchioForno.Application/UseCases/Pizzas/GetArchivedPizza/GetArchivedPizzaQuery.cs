using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.GetArchivedPizza;

public sealed record GetArchivedPizzaQuery(
    int Id
) : IRequest<ArchivedPizzaDto?>;