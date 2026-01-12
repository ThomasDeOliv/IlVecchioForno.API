using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase;