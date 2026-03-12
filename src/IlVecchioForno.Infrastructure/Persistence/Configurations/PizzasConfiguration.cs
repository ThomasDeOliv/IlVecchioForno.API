using IlVecchioForno.Domain.Pizzas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IlVecchioForno.Infrastructure.Persistence.Configurations;

internal class PizzasConfiguration : EntityConfigurationBase<Pizza>
{
    public override void Configure(EntityTypeBuilder<Pizza> builder)
    {
        base.Configure(builder);

        builder.ToTable("pizzas", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("ck_pizzas_name_maxlength", $"LENGTH(name) >= {PizzaInvariant.NameMinLength} AND LENGTH(name) <= {PizzaInvariant.NameMaxLength}");
            tableBuilder.HasCheckConstraint("ck_pizzas_description_maxlength", $"description IS NULL OR (LENGTH(description) >= {PizzaInvariant.DescriptionMinLength} AND LENGTH(description) <= {PizzaInvariant.DescriptionMaxLength})");
        });

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("INTEGER")
            .ValueGeneratedOnAdd()
            .UseIdentityByDefaultColumn();

        builder.Property(e => e.Name)
            .HasConversion(
                valueObject => valueObject.Value,
                value => new PizzaName(value)
            )
            .HasColumnName("name")
            .HasColumnType("CITEXT")
            .HasMaxLength(PizzaInvariant.NameMaxLength)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasConversion(
                valueObject => valueObject != null ? valueObject.Value : null,
                value => !string.IsNullOrEmpty(value) ? new PizzaDescription(value) : null
            )
            .HasColumnName("description")
            .HasColumnType("CITEXT")
            .HasMaxLength(PizzaInvariant.DescriptionMaxLength)
            .IsRequired(false);

        builder.Property(e => e.ArchivedAt)
            .HasColumnName("archived_at")
            .HasColumnType("TIMESTAMPTZ")
            .IsRequired(false);

        builder.Property(e => e.Price)
            .HasConversion(
                valueObject => valueObject.Value,
                value => new PizzaPrice(value)
            )
            .HasColumnName("price")
            .HasColumnType("NUMERIC(6, 2)")
            .IsRequired();

        builder.HasKey(e => e.Id)
            .HasName("pk_pizzas");
    }
}