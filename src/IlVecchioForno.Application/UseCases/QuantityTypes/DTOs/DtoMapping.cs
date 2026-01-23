using IlVecchioForno.Domain.QuantityTypes;
using Mapster;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;

internal sealed class DtoMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<QuantityType, QuantityTypeDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Unit, src => src.Unit != null ? src.Unit.Value : null);
    }
}