using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Domain.Ingredients;

namespace IlVecchioForno.Application.Gateways.Persistence;

public interface IIngredientRepository
{
    Task<IReadOnlyCollection<Ingredient>> ListAsync(
        QuerySpec<IngredientsSorter> query,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<Ingredient>> ResolveAsync(
        IEnumerable<int> ids,
        CancellationToken cancellationToken = default
    );

    void Add(Ingredient newIngredient);
}