using IlVecchioForno.API.Controllers;
using IlVecchioForno.Application.UseCases.Pizzas;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters.Pizzas;

public interface IApiPizzaPresenter : IPizzaPresenter
{
    ActionResult Result { get; }
    void Initialize(PizzasController controller);
}