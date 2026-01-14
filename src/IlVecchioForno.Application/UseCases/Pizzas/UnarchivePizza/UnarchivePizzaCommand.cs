using IlVecchioForno.Application.Common;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.UnarchivePizza;

public sealed record UnarchivePizzaCommand(
    int Id,
    bool ToArchive
) : IRequest<Result<int>>;