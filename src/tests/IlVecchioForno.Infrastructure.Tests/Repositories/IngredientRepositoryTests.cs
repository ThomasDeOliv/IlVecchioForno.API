using IlVecchioForno.Application.Gateways.Persistence;

namespace IlVecchioForno.Infrastructure.Tests.Repositories;

public sealed class IngredientRepositoryTests : EmptyInfrastructureTestsBase
{
    private readonly IIngredientRepository _repository;

    public IngredientRepositoryTests(DbContextFixture dbCtxFixture) : base(dbCtxFixture)
    {
        this._repository = null!; // new EfIngredientRepository();
    }
}