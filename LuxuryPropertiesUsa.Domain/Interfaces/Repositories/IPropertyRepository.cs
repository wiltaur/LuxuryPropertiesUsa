using LuxuryPropertiesUsa.Domain.Entities;

namespace LuxuryPropertiesUsa.Domain.Interfaces.Repositories;

public interface IPropertyRepository
{
    Task AddAsync(Property property, CancellationToken cancellationToken);
    Task<Property?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<Property>> GetAllFilteredAsync(bool sortOrderDesc, string searchString, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<int> GetTotalRecordsAsync(string searchString, CancellationToken cancellationToken);
}