using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly IMediator _mediator;

    protected ApiControllerBase(IMediator mediator)
    {
        this._mediator = mediator;
    }
}