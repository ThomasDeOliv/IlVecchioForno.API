using IlVecchioForno.Application.Common;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ArchivePizza;

public sealed record ArchivePizzaCommand(
    int Id
) : IRequest<Result<int>>;