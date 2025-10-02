using LuxuryPropertiesUsa.Domain.Interfaces;
using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;
using LuxuryPropertiesUsa.Infrastructure.Data;

namespace LuxuryPropertiesUsa.Infrastructure;
public class UnitOfWork(AppDbContext context, IPropertyRepository propertyRepository, IPropertyImageRepository propertyImageRepository) : IUnitOfWork
{
    public IPropertyRepository Properties { get; } = propertyRepository;
    public IPropertyImageRepository PropertyImages { get; } = propertyImageRepository;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) => await context.SaveChangesAsync(cancellationToken);
}