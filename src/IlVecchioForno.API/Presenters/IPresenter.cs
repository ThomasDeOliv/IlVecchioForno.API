using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters;

public interface IPresenter
{
    ActionResult Result { get; }
    void Initialize(string actionName, string controllerName);
}