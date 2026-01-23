namespace IlVecchioForno.API.Requests.Ingredient;

public sealed record RegisterIngredientRequest(
    string Name,
    short QuantityTypeId
);