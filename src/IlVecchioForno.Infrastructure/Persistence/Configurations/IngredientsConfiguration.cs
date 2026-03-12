using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.QuantityTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IlVecchioForno.Infrastructure.Persistence.Configurations;

internal class IngredientsConfiguration : EntityConfigurationBase<Ingredient>
{
    public override void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        base.Configure(builder);

        builder.ToTable("ingredients");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("INTEGER")
            .ValueGeneratedOnAdd()
            .UseIdentityByDefaultColumn();

        builder.Property(e => e.Name)
            .HasConversion(
                valueObject => valueObject.Value,
                value => new IngredientName(value)
            )
            .HasColumnName("name")
            .HasColumnType($"VARCHAR({IngredientInvariant.NameMaxLength})")
            .IsRequired();

        builder.Property<short?>("QuantityTypeId") // Shadow QuantityType FK
            .HasColumnName("quantity_type_id")
            .HasColumnType("SMALLINT")
            .IsRequired(false);

        builder.HasKey(e => e.Id)
            .HasName("pk_ingredients");

        builder.HasOne<QuantityType>(pi => pi.QuantityType)
            .WithMany() // No properties
            .HasForeignKey("QuantityTypeId") // Shadow QuantityType property
            .HasConstraintName("fk_ingredients__quantity_types")
            .OnDelete(DeleteBehavior.Restrict);
    }
}