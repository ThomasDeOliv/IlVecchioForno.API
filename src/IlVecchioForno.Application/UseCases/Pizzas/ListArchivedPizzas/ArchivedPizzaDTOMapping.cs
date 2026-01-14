using IlVecchioForno.Domain.Pizzas;
using Mapster;

namespace IlVecchioForno.Application.UseCases.Pizzas.ListArchivedPizzas;

internal sealed class ArchivedPizzaDTOMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Pizza, ArchivedPizzaDTO>()
            .MapWith(src =>
                new ArchivedPizzaDTO(
                    src.Id,
                    src.Name,
                    src.Description != null
                        ? src.Description.Value
                        : null,
                    src.Price,
                    src.Archived!.Value,
                    src.PizzaIngredients
                        .Select(pi =>
                            new ArchivedPizzaIngredientDTO(
                                pi.Quantity.Value,
                                pi.Ingredient.Id
                            )
                        )
                        .ToList()
                )
            );
    }
}