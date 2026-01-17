using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Pizzas;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.UnarchivePizza;

internal sealed class UnarchivePizzaHandler : IRequestHandler<UnarchivePizzaCommand, Result<int>>
{
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnarchivePizzaHandler(
        IPizzaRepository pizzaRepository,
        IUnitOfWork unitOfWork
    )
    {
        this._pizzaRepository = pizzaRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(UnarchivePizzaCommand request, CancellationToken cancellationToken)
    {
        Pizza? target = await this._pizzaRepository.FindAsync(request.Id, cancellationToken);

        if (target is null)
            return Result<int>.NotFound("Pizza not found.");

        target.UpdateArchived();
        int result = await this._unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<int>.Ok(result);
    }
}