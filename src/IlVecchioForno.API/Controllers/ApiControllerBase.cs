using IlVecchioForno.API.Presenters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly IMediator _mediator;
    protected readonly IPresenter _presenter;

    protected ApiControllerBase(IMediator mediator, IPresenter presenter)
    {
        this._mediator = mediator;
        this._presenter = presenter;
    }
}