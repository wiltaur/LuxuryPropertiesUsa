using LuxuryPropertiesUsa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxuryPropertiesUsa.Infrastructure.Configurations;

public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> entity)
    {
        entity.HasKey(e => e.IdOwner);

        entity.ToTable("Owner");

        entity.Property(e => e.Address).HasMaxLength(100);
        entity.Property(e => e.Birthday).HasColumnType("date");
        entity.Property(e => e.Name).HasMaxLength(100);
    }
}