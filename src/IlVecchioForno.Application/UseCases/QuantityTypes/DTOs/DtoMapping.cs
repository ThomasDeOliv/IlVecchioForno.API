using IlVecchioForno.Domain.QuantityTypes;
using Mapster;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;

internal sealed class DtoMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<QuantityType, QuantityTypeDto>()
            .MapWith(src => ToQuantityTypeDto(src));
    }

    private static QuantityTypeDto ToQuantityTypeDto(QuantityType src)
    {
        return new QuantityTypeDto(
            src.Id,
            src.Name.Value,
            src.Unit.Value
        );
    }
}