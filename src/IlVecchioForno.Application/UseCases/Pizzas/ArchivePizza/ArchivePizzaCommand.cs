using IlVecchioForno.Application.Common.Responses;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ArchivePizza;

public sealed record ArchivePizzaCommand(
    int Id
) : IRequest<IResponse>;