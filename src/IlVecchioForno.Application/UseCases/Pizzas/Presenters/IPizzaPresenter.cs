using IlVecchioForno.Application.Common.DTOs;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;

namespace IlVecchioForno.Application.UseCases.Pizzas.Presenters;

public interface IPizzaPresenter
{
    void EntityFound(ActivePizzaDto entity);
    void EntitiesListed(IReadOnlyList<ActivePizzaDto> entities);
    void EntityFound(ArchivedPizzaDto entity);
    void EntitiesListed(IReadOnlyList<ArchivedPizzaDto> entities);
    void EntitiesCount(EntitiesCountDto count);
    void EntityRegistered(ActivePizzaDto entity);
    void EntityUpdated();
    void RegistrationError(string message);
    void InvalidReferenceError(string message);
    void EntityNotFound(string message);
    void ValidationErrors(Dictionary<string, string[]> errors);
}