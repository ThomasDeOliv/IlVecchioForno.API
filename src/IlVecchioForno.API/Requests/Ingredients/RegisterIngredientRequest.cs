namespace IlVecchioForno.API.Requests.Ingredients;

public sealed record RegisterIngredientRequest(
    string Name,
    short QuantityTypeId
);