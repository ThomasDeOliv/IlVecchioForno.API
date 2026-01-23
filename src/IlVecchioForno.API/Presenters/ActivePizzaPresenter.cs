using IlVecchioForno.API.Resources.ActivePizza;
using IlVecchioForno.API.Resources.PizzaIngredient;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;

namespace IlVecchioForno.API.Presenters;

public sealed class ActivePizzaPresenter : IPresenter<ActivePizzaDto, ActivePizzaResource>
{
    public ActivePizzaResource Present(ActivePizzaDto response)
    {
        return new ActivePizzaResource(
            response.Id,
            response.Name,
            response.Description,
            response.Price,
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