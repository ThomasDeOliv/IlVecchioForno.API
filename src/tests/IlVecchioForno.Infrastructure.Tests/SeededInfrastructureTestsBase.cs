using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;

namespace IlVecchioForno.Infrastructure.Tests;

public abstract class SeededInfrastructureTestsBase : EmptyInfrastructureTestsBase
{
    protected SeededInfrastructureTestsBase(DbContextFixture dbCtxFixture) : base(dbCtxFixture)
    {
    }

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        // Need deterministic insert
        foreach (QuantityType qT in DbMockedTestsData.TestsQuantityTypes)
        {
            this._ctx.QuantityTypes.Add(qT);
            await this._ctx.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        // Need deterministic insert
        foreach (Ingredient i in DbMockedTestsData.TestsIngredients)
        {
            this._ctx.Ingredients.Add(i);
            await this._ctx.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        // Need deterministic insert
        foreach (Pizza p in DbMockedTestsData.TestsPizzas)
        {
            this._ctx.Pizzas.Add(p);
            await this._ctx.SaveChangesAsync(TestContext.Current.CancellationToken);
        }
    }
}