using LuxuryPropertiesUsa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxuryPropertiesUsa.Infrastructure.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> entity)
    {
        entity.HasKey(e => e.IdProperty);

        entity.ToTable("Property");

        entity.HasIndex(e => e.CodeInternal, "IX_Code_Internal").IsUnique();

        entity.Property(e => e.Address).HasMaxLength(100);
        entity.Property(e => e.Name).HasMaxLength(50);
        entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        entity.Property(e => e.Year).HasColumnType("numeric(4, 0)");

        entity.HasOne(d => d.IdOwnerNavigation).WithMany(p => p.Properties)
            .HasForeignKey(d => d.IdOwner)
            .HasConstraintName("FK_Property_Owner");
    }
}