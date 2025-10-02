using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;
using LuxuryPropertiesUsa.Infrastructure.Data;

namespace LuxuryPropertiesUsa.Infrastructure.Repositories;
public class PropertyImageRepository(AppDbContext context) : IPropertyImageRepository
{
    public async Task AddRangeAsync(List<PropertyImage> propertyImages, CancellationToken cancellationToken)
        => await context.PropertyImages.AddRangeAsync(propertyImages, cancellationToken);
}