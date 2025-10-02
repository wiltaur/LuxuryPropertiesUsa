using LuxuryPropertiesUsa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxuryPropertiesUsa.Infrastructure.Configurations;

public class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
{
    public void Configure(EntityTypeBuilder<PropertyImage> entity)
    {
        entity.HasKey(e => e.IdPropertyImage);

        entity.ToTable("PropertyImage");

        entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.PropertyImages)
            .HasForeignKey(d => d.IdProperty)
            .HasConstraintName("FK_PropertyImage_Property");
    }
}