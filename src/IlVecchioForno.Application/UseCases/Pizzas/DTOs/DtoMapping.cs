using IlVecchioForno.Domain.Pizzas;
using Mapster;

namespace IlVecchioForno.Application.UseCases.Pizzas.DTOs;

internal sealed class DtoMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Pizza, ActivePizzaDto>()
            .MapWith(src =>
                new ActivePizzaDto(
                    src.Id,
                    src.Name.Value,
                    src.Description != null
                        ? src.Description.Value
                        : null,
                    src.Price,
                    src.PizzaIngredients
                        .Select(pi =>
                            new PizzaIngredientDto(
                                pi.Quantity.Value,
                                pi.Ingredient.Id
                            )
                        )
                        .ToList()
                )
            );

        config.NewConfig<Pizza, ArchivedPizzaDto>()
            .MapWith(src =>
                new ArchivedPizzaDto(
                    src.Id,
                    src.Name,
                    src.Description != null
                        ? src.Description.Value
                        : null,
                    src.Price,
                    src.ArchivedAt!.Value,
                    src.PizzaIngredients
                        .Select(pi =>
                            new PizzaIngredientDto(
                                pi.Quantity.Value,
                                pi.Ingredient.Id
                            )
                        )
                        .ToList()
                )
            );
    }
}