using IlVecchioForno.Application.Gateways.Persistence;

namespace IlVecchioForno.Infrastructure.Tests.Repositories;

public sealed class PizzaRepositoryTests : InfrastructureTestsBase
{
    private readonly IPizzaRepository _repository;

    public PizzaRepositoryTests(DbContextFixture dbCtxFixture) : base(dbCtxFixture)
    {
        this._repository = null!; // new EfPizzaRepository();
    }
}