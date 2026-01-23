using IlVecchioForno.API.Resources.Ingredient;
using IlVecchioForno.Application.UseCases.Ingredients.DTOs;

namespace IlVecchioForno.API.Presenters;

public sealed class IngredientPresenter : IPresenter<IngredientDto, IngredientResource>
{
    public IngredientResource Present(IngredientDto response)
    {
        return new IngredientResource(
            response.Id,
            response.Name,
            response.QuantityTypeId
        );
    }
}