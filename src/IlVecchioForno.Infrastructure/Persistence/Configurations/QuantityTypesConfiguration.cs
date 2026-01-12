using IlVecchioForno.Domain.QuantityTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IlVecchioForno.Infrastructure.Persistence.Configurations;

internal class QuantityTypesConfiguration : EntityConfigurationBase<QuantityType>
{
    public override void Configure(EntityTypeBuilder<QuantityType> builder)
    {
        base.Configure(builder);

        builder.ToTable("quantity_types");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasColumnType("SMALLINT")
            .ValueGeneratedOnAdd()
            .UseIdentityByDefaultColumn();

        builder.Property(e => e.Name)
            .HasConversion(
                valueObject => valueObject.Value,
                value => new QuantityTypeName(value)
            )
            .HasColumnName("name")
            .HasColumnType($"VARCHAR({QuantityTypeInvariant.NameMaxLength})")
            .IsRequired();

        builder.Property(e => e.Unit)
            .HasConversion(
                valueObject => valueObject != null ? valueObject.Value : null,
                value => !string.IsNullOrEmpty(value) ? new QuantityTypeUnit(value) : null
            )
            .HasColumnName("unit")
            .HasColumnType($"VARCHAR({QuantityTypeInvariant.UnitMaxLength})")
            .IsRequired(false);

        builder.HasKey(e => e.Id)
            .HasName("pk_quantity_types");
    }
}