using IlVecchioForno.Application.Common.DTOs;

namespace IlVecchioForno.Application.UseCases.Ingredients;

public interface IIngredientPresenter
{
    void EntityFound(IngredientDto entity);
    void EntitiesListed(IReadOnlyList<IngredientDto> entities);
    void EntitiesCount(EntitiesCountDto count);
    void EntityRegistered(IngredientDto entity);
    void RegistrationError(string message);
    void InvalidReferenceError(string message);
    void EntityNotFound(string message);
    void ValidationErrors(Dictionary<string, string[]> errors);
}