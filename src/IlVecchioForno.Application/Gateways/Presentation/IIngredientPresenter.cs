using IlVecchioForno.Application.UseCases.Ingredients.DTOs;

namespace IlVecchioForno.Application.Gateways.Presentation;

public interface IIngredientPresenter
{
    void EntityFound(IngredientDto entity);
    void EntitiesListed(IReadOnlyList<IngredientDto> entities);
    void EntityRegistered(IngredientDto entity);
    void RegistrationError(string message);
    void InvalidReferenceError(string message);
    void EntityNotFound(string message);
    void ValidationErrors(Dictionary<string, string[]> errors);
}