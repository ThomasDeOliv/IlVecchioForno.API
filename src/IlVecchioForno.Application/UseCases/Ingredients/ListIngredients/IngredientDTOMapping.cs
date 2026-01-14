using IlVecchioForno.Domain.Ingredients;
using Mapster;

namespace IlVecchioForno.Application.UseCases.Ingredients.ListIngredients;

internal sealed class IngredientDTOMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Ingredient, IngredientDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.QuantityTypeId, src => src.QuantityType.Id);
    }
}