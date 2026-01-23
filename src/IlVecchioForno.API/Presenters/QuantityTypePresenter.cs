using IlVecchioForno.API.Resources.QuantityType;
using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;

namespace IlVecchioForno.API.Presenters;

public sealed class QuantityTypePresenter : IPresenter<QuantityTypeDto, QuantityTypeResource>
{
    public QuantityTypeResource Present(QuantityTypeDto response)
    {
        return new QuantityTypeResource(
            response.Id,
            response.Name,
            response.Unit
        );
    }
}