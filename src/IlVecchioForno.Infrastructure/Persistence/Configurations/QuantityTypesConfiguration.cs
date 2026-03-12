using IlVecchioForno.Domain.QuantityTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IlVecchioForno.Infrastructure.Persistence.Configurations;

internal class QuantityTypesConfiguration : EntityConfigurationBase<QuantityType>
{
    public override void Configure(EntityTypeBuilder<QuantityType> builder)
    {
        base.Configure(builder);

        builder.ToTable("quantity_types", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("ck_quantity_types_name_maxlength", $"LENGTH(name) >= {QuantityTypeInvariant.NameMinLength} AND LENGTH(name) <= {QuantityTypeInvariant.NameMaxLength}");
            tableBuilder.HasCheckConstraint("ck_quantity_types_unit_maxlength", $"LENGTH(unit) >= {QuantityTypeInvariant.UnitMinLength} AND LENGTH(unit) <= {QuantityTypeInvariant.UnitMaxLength}");
        });

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
            .HasColumnType("CITEXT")
            .HasMaxLength(QuantityTypeInvariant.NameMaxLength)
            .IsRequired();

        builder.Property(e => e.Unit)
            .HasConversion(
                valueObject => valueObject.Value,
                value => new QuantityTypeUnit(value)
            )
            .HasColumnName("unit")
            .HasColumnType("CITEXT")
            .HasMaxLength(QuantityTypeInvariant.UnitMaxLength)
            .IsRequired();

        builder.HasKey(e => e.Id)
            .HasName("pk_quantity_types");
    }
}