using IlVecchioForno.Application.Gateways.Persistence;

namespace IlVecchioForno.Infrastructure.Tests.Repositories;

public sealed class QuantityTypeRepositoryTests : EmptyInfrastructureTestsBase
{
    private readonly IQuantityTypeRepository _repository;

    public QuantityTypeRepositoryTests(DbContextFixture dbCtxFixture) : base(dbCtxFixture)
    {
        this._repository = null!; // new EfQuantityTypeRepository();
    }
}