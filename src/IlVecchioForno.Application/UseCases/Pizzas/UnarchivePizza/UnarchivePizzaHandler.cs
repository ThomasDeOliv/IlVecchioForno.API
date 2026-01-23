using IlVecchioForno.Application.Common.Exceptions;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Pizzas;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.UnarchivePizza;

internal sealed class UnarchivePizzaHandler : IRequestHandler<UnarchivePizzaCommand, Unit>
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

    public async Task<Unit> Handle(UnarchivePizzaCommand request, CancellationToken cancellationToken)
    {
        Pizza? target = await this._pizzaRepository.FindAsync(request.Id, cancellationToken);

        if (target is null)
            throw new InvalidReferenceException("Pizza not found.");

        target.UpdateArchived();
        await this._unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}