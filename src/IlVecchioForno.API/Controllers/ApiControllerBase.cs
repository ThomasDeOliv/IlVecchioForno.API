using IlVecchioForno.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult<Result<T>> ToActionResult<T>(Result<T> result)
    {
        return result.ErrorType switch
        {
            ResultErrorType.None => this.Ok(result),
            ResultErrorType.ValidationError => this.BadRequest(result),
            ResultErrorType.NotFound => this.NotFound(result),
            ResultErrorType.Conflict => this.Conflict(result),
            ResultErrorType.Unauthorized => this.Unauthorized(result),
            ResultErrorType.Forbidden => this.Forbid(),
            _ => this.StatusCode(500, result)
        };
    }
}