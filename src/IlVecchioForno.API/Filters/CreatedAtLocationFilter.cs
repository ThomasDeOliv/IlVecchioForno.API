using IlVecchioForno.API.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Primitives;

namespace IlVecchioForno.API.Filters;

public sealed class CreatedAtLocationFilter : IResultFilter
{
    private readonly IUrlHelperFactory _urlHelperFactory;

    public CreatedAtLocationFilter(IUrlHelperFactory urlHelperFactory)
    {
        this._urlHelperFactory = urlHelperFactory;
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not ObjectResult { StatusCode: StatusCodes.Status201Created, Value: not null } result)
            return;

        CreatedAtActionAttribute? attribute = context.ActionDescriptor.EndpointMetadata
            .OfType<CreatedAtActionAttribute>()
            .FirstOrDefault();

        if (attribute is null)
            return;

        if (context.ActionDescriptor is not ControllerActionDescriptor controllerDescriptor)
            return;

        object? id = result.Value.GetType()
            .GetProperty("Id")?
            .GetValue(result.Value);

        if (id is null)
            return;

        IUrlHelper urlHelper = this._urlHelperFactory.GetUrlHelper(context);

        string? location = urlHelper.Action(
            attribute.ActionName,
            controllerDescriptor.ControllerName,
            new { id }
        );

        if (location is not null)
            context.HttpContext.Response.Headers.Location = new StringValues(location);
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}