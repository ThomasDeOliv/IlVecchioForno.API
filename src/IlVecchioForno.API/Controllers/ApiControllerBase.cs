using IlVecchioForno.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult<Result<T>> ToActionResult<T>(Result<T> result)
    {
        return result.Type switch
        {
            ResultType.Ok => this.Ok(result),
            ResultType.Created => this.StatusCode(201, result), // TODO : this.Created(...) with URI later maybe
            ResultType.ValidationError => this.BadRequest(result),
            ResultType.NotFound => this.NotFound(result),
            ResultType.Conflict => this.Conflict(result),
            ResultType.Unauthorized => this.Unauthorized(result),
            ResultType.Forbidden => this.StatusCode(403, result),
            _ => this.StatusCode(500, result)
        };
    }
}