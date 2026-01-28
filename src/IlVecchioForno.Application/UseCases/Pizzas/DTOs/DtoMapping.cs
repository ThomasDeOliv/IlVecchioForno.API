using IlVecchioForno.Application.Common.Exceptions;
using IlVecchioForno.Domain.Pizzas;
using Mapster;

namespace IlVecchioForno.Application.UseCases.Pizzas.DTOs;

internal sealed class DtoMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Pizza, ActivePizzaDto>()
            .MapWith(src => ToActivePizzaDto(src));

        config.NewConfig<Pizza, ArchivedPizzaDto>()
            .MapWith(src => ToArchivedPizzaDto(src));
    }

    private static ActivePizzaDto ToActivePizzaDto(Pizza src)
    {
        if (src.ArchivedAt.HasValue)
            throw new MappingException("Cannot map an archived pizza to ActivePizzaDto.");

        return new ActivePizzaDto(
            src.Id,
            src.Name.Value,
            src.Description?.Value,
            src.Price.Value,
            src.PizzaIngredients
                .Select(pi =>
                    new PizzaIngredientDto(
                        pi.Quantity.Value,
                        pi.Ingredient.Id
                    )
                )
                .ToList()
        );
    }

    private static ArchivedPizzaDto ToArchivedPizzaDto(Pizza src)
    {
        if (!src.ArchivedAt.HasValue)
            throw new MappingException("Cannot map a non-archived pizza to ArchivedPizzaDto.");

        return new ArchivedPizzaDto(
            src.Id,
            src.Name.Value,
            src.Description?.Value,
            src.Price.Value,
            src.ArchivedAt.Value,
            src.PizzaIngredients
                .Select(pi =>
                    new PizzaIngredientDto(
                        pi.Quantity.Value,
                        pi.Ingredient.Id
                    )
                )
                .ToList()
        );
    }
}