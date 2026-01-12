using IlVecchioForno.Domain.Pizzas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IlVecchioForno.Infrastructure.Persistence.Configurations;

internal class PizzasConfiguration : EntityConfigurationBase<Pizza>
{
    public override void Configure(EntityTypeBuilder<Pizza> builder)
    {
        base.Configure(builder);

        builder.ToTable("pizzas");

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
            .HasColumnType("VARCHAR(256)")
            .IsRequired();

        builder.Property(e => e.Description)
            .HasConversion(
                valueObject => valueObject != null ? valueObject.Value : null,
                value => !string.IsNullOrEmpty(value) ? new PizzaDescription(value) : null
            )
            .HasColumnName("description")
            .HasColumnType("TEXT")
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(e => e.Archived)
            .HasColumnName("archived")
            .HasColumnType("TIMESTAMPTZ")
            .IsRequired(false);

        builder.Property(e => e.Price)
            .HasConversion(
                valueObject => valueObject.Value,
                value => new PizzaPrice(value)
            )
            .HasColumnName("price")
            .HasColumnType("NUMERIC(18,2)")
            .IsRequired();

        builder.HasKey(e => e.Id)
            .HasName("pk_pizzas");
    }
}