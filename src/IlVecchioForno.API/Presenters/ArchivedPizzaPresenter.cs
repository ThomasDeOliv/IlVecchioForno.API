using IlVecchioForno.API.Resources.ArchivedPizza;
using IlVecchioForno.API.Resources.PizzaIngredient;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;

namespace IlVecchioForno.API.Presenters;

public sealed class ArchivedPizzaPresenter : IPresenter<ArchivedPizzaDto, ArchivedPizzaResource>
{
    public ArchivedPizzaResource Present(ArchivedPizzaDto response)
    {
        return new ArchivedPizzaResource(
            response.Id,
            response.Name,
            response.Description,
            response.Price,
            response.ArchivedAt,
            response.Ingredients
                .Select(i =>
                    new PizzaIngredientResource(
                        i.Quantity,
                        i.IngredientId
                    )
                ).ToList()
        );
    }
}