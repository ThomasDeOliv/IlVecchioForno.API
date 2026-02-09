using IlVecchioForno.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters;

public abstract class PresenterBase : IPresenter
{
    private string? _actionName;
    private string? _controllerName;
    protected ActionResult? _result;

    protected PresenterBase()
    {
        this._actionName = null;
        this._controllerName = null;
        this._result = null;
    }

    protected string ActionName =>
        this._actionName
        ?? throw new PresenterActionNameNotSetException();

    protected string ControllerName =>
        this._controllerName
        ?? throw new PresenterControllerNameNotSetException();

    public ActionResult Result =>
        this._result
        ?? throw new PresenterResultNotSetException();

    public void Initialize(string actionName, string controllerName)
    {
        this._actionName = actionName;
        this._controllerName = controllerName;
    }
}