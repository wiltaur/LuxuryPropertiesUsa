using LuxuryPropertiesUsa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxuryPropertiesUsa.Infrastructure.Configurations;

public class PropertyTraceConfiguration : IEntityTypeConfiguration<PropertyTrace>
{
    public void Configure(EntityTypeBuilder<PropertyTrace> entity)
    {
        entity.HasKey(e => e.IdPropertyTrace);

        entity.ToTable("PropertyTrace");

        entity.Property(e => e.DateSale).HasColumnType("datetime");
        entity.Property(e => e.Name).HasMaxLength(50);
        entity.Property(e => e.Tax).HasColumnType("decimal(4, 2)");
        entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");

        entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.PropertyTraces)
            .HasForeignKey(d => d.IdProperty)
            .HasConstraintName("FK_PropertyTrace_Property");
    }
}