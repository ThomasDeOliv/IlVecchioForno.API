using IlVecchioForno.Application.UseCases.Pizzas.DTOs;

namespace IlVecchioForno.Application.Gateways.Presentation;

public interface IPizzaPresenter
{
    void EntityFound(ActivePizzaDto entity);
    void EntitiesListed(IReadOnlyList<ActivePizzaDto> entities);
    void EntityFound(ArchivedPizzaDto entity);
    void EntitiesListed(IReadOnlyList<ArchivedPizzaDto> entities);
    void EntityRegistered(ActivePizzaDto entity);
    void EntityUpdated();
    void RegistrationError(string message);
    void InvalidReferenceError(string message);
    void EntityNotFound(string message);
    void ValidationErrors(Dictionary<string, string[]> errors);
}