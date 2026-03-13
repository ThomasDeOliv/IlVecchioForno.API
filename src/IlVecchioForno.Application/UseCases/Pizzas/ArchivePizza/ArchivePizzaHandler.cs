using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Pizzas;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ArchivePizza;

internal sealed class ArchivePizzaHandler : IRequestHandler<ArchivePizzaCommand>
{
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IPizzaPresenter _presenter;
    private readonly IUnitOfWork _unitOfWork;

    public ArchivePizzaHandler(
        IPizzaPresenter presenter,
        IPizzaRepository pizzaRepository,
        IUnitOfWork unitOfWork
    )
    {
        this._presenter = presenter;
        this._pizzaRepository = pizzaRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ArchivePizzaCommand command,
        CancellationToken cancellationToken
    )
    {
        Pizza? target = await this._pizzaRepository.FindAsync(command.Id, cancellationToken);

        if (target is null)
        {
            this._presenter.InvalidReferenceError("Pizza not found.");
            return;
        }

        target.UpdateArchived();
        await this._unitOfWork.SaveChangesAsync(cancellationToken);

        this._presenter.EntityUpdated();
    }
}