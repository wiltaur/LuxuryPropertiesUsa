using LuxuryPropertiesUsa.Domain.Interfaces;
using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;
using LuxuryPropertiesUsa.Infrastructure.Data;

namespace LuxuryPropertiesUsa.Infrastructure;
public class UnitOfWork(AppDbContext context, IPropertyRepository propertyRepository) : IUnitOfWork
{
    public IPropertyRepository Properties { get; } = propertyRepository;

    public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();
}