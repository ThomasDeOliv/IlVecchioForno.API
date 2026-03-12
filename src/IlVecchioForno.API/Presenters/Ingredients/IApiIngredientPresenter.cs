using IlVecchioForno.API.Controllers;
using IlVecchioForno.Application.UseCases.Ingredients.Presenters;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters.Ingredients;

public interface IApiIngredientPresenter : IIngredientPresenter
{
    ActionResult Result { get; }
    void Initialize(IngredientsController controller);
}