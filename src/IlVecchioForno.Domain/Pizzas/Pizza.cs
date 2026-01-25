using IlVecchioForno.Domain.Common;
using IlVecchioForno.Domain.PizzaIngredients;
using IlVecchioForno.Domain.Pizzas.Exceptions;

namespace IlVecchioForno.Domain.Pizzas;

public sealed class Pizza : EntityBase
{
    private readonly List<PizzaIngredient> _pizzaIngredients;

    // EF
    private Pizza() : base()
    {
        this._pizzaIngredients = new List<PizzaIngredient>();

        this.Id = 0;
        this.Name = null!;
        this.Description = null;
        this.Price = null!;
        this.ArchivedAt = null;
    }

    public Pizza(PizzaName name, PizzaDescription? description, PizzaPrice price, int id = 0) : this()
    {
        this.Id = id;
        this.Name = name;
        this.Description = description;
        this.Price = price;
    }

    public int Id { get; private set; }
    public PizzaName Name { get; private set; }
    public PizzaDescription? Description { get; private set; }
    public PizzaPrice Price { get; private set; }
    public DateTimeOffset? ArchivedAt { get; private set; }
    public IReadOnlyCollection<PizzaIngredient> PizzaIngredients => this._pizzaIngredients.AsReadOnly();

    public void UpdateName(PizzaName value)
    {
        this.Name = value;
    }

    public void UpdateDescription(PizzaDescription? description)
    {
        this.Description = description;
    }

    public void UpdatePrice(PizzaPrice price)
    {
        this.Price = price;
    }

    public void UpdateArchived()
    {
        this.ArchivedAt = this.ArchivedAt is not null
            ? null
            : DateTimeOffset.UtcNow;
    }

    public void UpdateIngredients(IEnumerable<PizzaIngredient> ingredients)
    {
        List<PizzaIngredient> ingredientsList = ingredients.ToList();
        List<PizzaIngredient> distinctIngredientsList =
            ingredientsList.DistinctBy(i => i.Ingredient).ToList();

        if (ingredientsList.Count != distinctIngredientsList.Count)
            throw new PizzaAggregateBaseException("Provided ingredients must be unique.");

        this._pizzaIngredients.Clear();
        this._pizzaIngredients.AddRange(ingredientsList);
    }
}