using IlVecchioForno.Domain.QuantityTypes;
using Mapster;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.ListQuantityTypes;

internal sealed class QuantityTypeDTOMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<QuantityType, QuantityTypeDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Unit, src => src.Unit != null ? src.Unit.Value : null);
    }
}