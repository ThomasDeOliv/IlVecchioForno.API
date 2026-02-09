using IlVecchioForno.API.Controllers;
using IlVecchioForno.Application.Gateways.Presentation;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters.Ingredients;

public interface IApiIngredientPresenter : IIngredientPresenter
{
    ActionResult Result { get; }
    void Initialize(ApiControllerBase controller);
}