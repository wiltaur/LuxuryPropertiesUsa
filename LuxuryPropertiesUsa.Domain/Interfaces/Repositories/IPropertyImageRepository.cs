using LuxuryPropertiesUsa.Domain.Entities;

namespace LuxuryPropertiesUsa.Domain.Interfaces.Repositories;

public interface IPropertyImageRepository
{
    Task AddRangeAsync(List<PropertyImage> propertyImages, CancellationToken cancellationToken);
}