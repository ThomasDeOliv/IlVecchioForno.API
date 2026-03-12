using IlVecchioForno.API.Controllers;
using IlVecchioForno.Application.UseCases.QuantityTypes.Presenters;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters.QuantityTypes;

public interface IApiQuantityTypePresenter : IQuantityTypePresenter
{
    ActionResult Result { get; }
    void Initialize(QuantityTypesController controller);
}