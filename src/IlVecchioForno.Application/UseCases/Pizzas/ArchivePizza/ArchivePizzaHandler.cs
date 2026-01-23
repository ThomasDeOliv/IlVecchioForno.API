using IlVecchioForno.Application.Common.Exceptions;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Pizzas;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ArchivePizza;

internal sealed class ArchivePizzaHandler : IRequestHandler<ArchivePizzaCommand, Unit>
{
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ArchivePizzaHandler(
        IPizzaRepository pizzaRepository,
        IUnitOfWork unitOfWork
    )
    {
        this._pizzaRepository = pizzaRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ArchivePizzaCommand request, CancellationToken cancellationToken)
    {
        Pizza? target = await this._pizzaRepository.FindAsync(request.Id, cancellationToken);

        if (target is null)
            throw new InvalidReferenceException("Pizza not found.");

        target.UpdateArchived();
        await this._unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}