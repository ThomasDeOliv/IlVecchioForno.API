using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.PizzaIngredients;
using IlVecchioForno.Domain.Pizzas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IlVecchioForno.Infrastructure.Persistence.Configurations;

internal class PizzasIngredientsConfiguration : EntityConfigurationBase<PizzaIngredient>
{
    public override void Configure(EntityTypeBuilder<PizzaIngredient> builder)
    {
        base.Configure(builder);

        builder.ToTable("pizzas_ingredients");

        builder.Property(e => e.Quantity)
            .HasConversion(
                valueObject => valueObject.Value,
                value => new PizzaIngredientQuantity(value)
            )
            .HasColumnName("quantity")
            .HasColumnType("NUMERIC(9, 3)")
            .IsRequired();

        builder.Property<int>("PizzaId") // Shadow Pizza FK
            .HasColumnName("pizza_id")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.Property<int>("IngredientId") // Shadow Ingredient FK
            .HasColumnName("ingredient_id")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.HasKey("PizzaId", "IngredientId") // Composite PK with shadow properties
            .HasName("pk_pizzas_ingredients");

        // Nav
        builder.HasOne<Pizza>()
            .WithMany(p => p.PizzaIngredients)
            .HasForeignKey("PizzaId") // Shadow Pizza property
            .HasConstraintName("fk_pizzas_ingredients__pizzas")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Ingredient>(pi => pi.Ingredient)
            .WithMany() // No properties
            .HasForeignKey("IngredientId") // Shadow Ingredient property
            .HasConstraintName("fk_pizzas_ingredients__ingredients")
            .OnDelete(DeleteBehavior.Restrict);
    }
}