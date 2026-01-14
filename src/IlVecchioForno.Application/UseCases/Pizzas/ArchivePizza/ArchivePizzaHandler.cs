using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Pizzas;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ArchivePizza;

internal class ArchivePizzaHandler : IRequestHandler<ArchivePizzaCommand, Result<int>>
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

    public async Task<Result<int>> Handle(ArchivePizzaCommand request, CancellationToken cancellationToken)
    {
        Pizza? target = await this._pizzaRepository.FindAsync(request.Id, cancellationToken);

        if (target is null)
            return Result<int>.ValidationError("Provided pizza provided.");

        target.UpdateArchived();
        int result = await this._unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<int>.Ok(result);
    }
}