using LuxuryPropertiesUsa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LuxuryPropertiesUsa.Infrastructure.Data;

public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public virtual DbSet<Owner> Owners { get; set; }
    public virtual DbSet<Property> Properties { get; set; }
    public virtual DbSet<PropertyImage> PropertyImages { get; set; }
    public virtual DbSet<PropertyTrace> PropertyTraces { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}