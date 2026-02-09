using IlVecchioForno.Application.Gateways.Presentation;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters.QuantityTypes;

public interface IApiQuantityTypePresenter : IQuantityTypePresenter
{
    ActionResult Result { get; }
}