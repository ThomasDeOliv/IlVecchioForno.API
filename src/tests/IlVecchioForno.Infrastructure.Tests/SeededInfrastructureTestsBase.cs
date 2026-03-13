using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;

namespace IlVecchioForno.Infrastructure.Tests;

public abstract class SeededInfrastructureTestsBase : EmptyInfrastructureTestsBase
{
    protected readonly List<Ingredient> _ingredients;
    protected readonly List<Pizza> _pizzas;
    protected readonly List<QuantityType> _quantityTypes;

    protected SeededInfrastructureTestsBase(DbContextFixture dbCtxFixture) : base(dbCtxFixture)
    {
        this._quantityTypes = RawMockedTestsData.CreateQuantityTypes();
        this._ingredients = RawMockedTestsData.CreateIngredients(this._quantityTypes);
        this._pizzas = RawMockedTestsData.CreatePizzas();
    }

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        // Need deterministic insert
        foreach (QuantityType qT in this._quantityTypes)
        {
            this._ctx.QuantityTypes.Add(qT);
            await this._ctx.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        // Need deterministic insert
        foreach (Ingredient i in this._ingredients)
        {
            this._ctx.Ingredients.Add(i);
            await this._ctx.SaveChangesAsync(TestContext.Current.CancellationToken);
        }

        // Need deterministic insert
        foreach (Pizza p in this._pizzas)
        {
            this._ctx.Pizzas.Add(p);
            await this._ctx.SaveChangesAsync(TestContext.Current.CancellationToken);
        }
    }
}