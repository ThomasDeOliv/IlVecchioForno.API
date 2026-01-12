using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.QuantityTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IlVecchioForno.Infrastructure.Persistence.Setup;

public static class DbInitExtensions
{
    extension(IApplicationBuilder app)
    {
        public async Task ApplyMigrationsAsync()
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            IlVecchioFornoDbContext ctx = scope.ServiceProvider.GetRequiredService<IlVecchioFornoDbContext>();
            await ctx.Database.MigrateAsync();
        }

        public async Task SeedDatabaseAsync()
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            IlVecchioFornoDbContext ctx = scope.ServiceProvider.GetRequiredService<IlVecchioFornoDbContext>();

            if (await ctx.QuantityTypes.AnyAsync() || await ctx.Ingredients.AnyAsync())
                return;

            List<QuantityType> quantityTypes = new List<QuantityType>
            {
                new QuantityType(new QuantityTypeName("Unit"), null),
                new QuantityType(new QuantityTypeName("Milligrams"), new QuantityTypeUnit("mg")),
                new QuantityType(new QuantityTypeName("Grams"), new QuantityTypeUnit("g")),
                new QuantityType(new QuantityTypeName("Kilograms"), new QuantityTypeUnit("kg")),
                new QuantityType(new QuantityTypeName("Milliliters"), new QuantityTypeUnit("mL")),
                new QuantityType(new QuantityTypeName("Centiliters"), new QuantityTypeUnit("cL")),
                new QuantityType(new QuantityTypeName("Liters"), new QuantityTypeUnit("L"))
            };

            List<Ingredient> ingredients = new List<Ingredient>
            {
                new Ingredient(new IngredientName("Flour (00)"), quantityTypes[2]),
                new Ingredient(new IngredientName("Water"), quantityTypes[4]),
                new Ingredient(new IngredientName("Fresh yeast"), quantityTypes[2]),
                new Ingredient(new IngredientName("Salt"), quantityTypes[2]),
                new Ingredient(new IngredientName("Extra virgin olive oil"), quantityTypes[4]),
                new Ingredient(new IngredientName("San Marzano tomato sauce"), quantityTypes[2]),
                new Ingredient(new IngredientName("Fresh basil leaves"), quantityTypes[0]),
                new Ingredient(new IngredientName("Garlic clove"), quantityTypes[0]),
                new Ingredient(new IngredientName("Oregano"), quantityTypes[2]),
                new Ingredient(new IngredientName("Mozzarella fior di latte"), quantityTypes[2]),
                new Ingredient(new IngredientName("Parmigiano Reggiano"), quantityTypes[2]),
                new Ingredient(new IngredientName("Pecorino Romano"), quantityTypes[2]),
                new Ingredient(new IngredientName("Prosciutto crudo"), quantityTypes[2]),
                new Ingredient(new IngredientName("Spianata calabra"), quantityTypes[2]),
                new Ingredient(new IngredientName("Pancetta"), quantityTypes[2]),
                new Ingredient(new IngredientName("'Nduja"), quantityTypes[2]),
                new Ingredient(new IngredientName("Black olives"), quantityTypes[0]),
                new Ingredient(new IngredientName("Mushrooms"), quantityTypes[2]),
                new Ingredient(new IngredientName("Artichoke hearts"), quantityTypes[0]),
                new Ingredient(new IngredientName("Capers"), quantityTypes[2]),
                new Ingredient(new IngredientName("Arugula"), quantityTypes[2]),
                new Ingredient(new IngredientName("Cherry tomatoes"), quantityTypes[2]),
                new Ingredient(new IngredientName("Buffalo mozzarella"), quantityTypes[2])
            };

            ctx.QuantityTypes.AddRange(quantityTypes);
            ctx.Ingredients.AddRange(ingredients);

            await ctx.SaveChangesAsync();
        }
    }
}