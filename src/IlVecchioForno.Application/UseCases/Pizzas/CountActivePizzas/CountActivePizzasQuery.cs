using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.CountActivePizzas;

public sealed record CountActivePizzasQuery(
    string? Search
) : IRequest;