using IlVecchioForno.Domain.Pizzas;
using Mapster;

namespace IlVecchioForno.Application.UseCases.Pizzas.ListActivePizzas;

internal sealed class ActivePizzaDTOMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Pizza, ActivePizzaDTO>()
            .MapWith(src =>
                new ActivePizzaDTO(
                    src.Id,
                    src.Name.Value,
                    src.Description != null
                        ? src.Description.Value
                        : null,
                    src.Price,
                    src.PizzaIngredients
                        .Select(pi =>
                            new ActivePizzaIngredientDTO(
                                pi.Quantity.Value,
                                pi.Ingredient.Id
                            )
                        )
                        .ToList()
                )
            );
    }
}