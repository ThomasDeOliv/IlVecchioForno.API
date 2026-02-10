using IlVecchioForno.Application.Common.DTOs;
using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;

namespace IlVecchioForno.Application.Gateways.Presentation;

public interface IQuantityTypePresenter
{
    void EntityFound(QuantityTypeDto entity);
    void EntitiesListed(IReadOnlyList<QuantityTypeDto> entities);
    void EntitiesCount(EntitiesCountDto count);
    void EntityNotFound(string message);
    void ValidationErrors(Dictionary<string, string[]> errors);
}