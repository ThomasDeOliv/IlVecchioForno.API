using IlVecchioForno.Domain.Ingredients;
using Mapster;

namespace IlVecchioForno.Application.UseCases.Ingredients;

internal sealed class DtoMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Ingredient, IngredientDto>()
            .MapWith(src => ToIngredientDto(src));
    }

    private static IngredientDto ToIngredientDto(Ingredient src)
    {
        return new IngredientDto(
            src.Id,
            src.Name.Value,
            src.QuantityType?.Id
        );
    }
}