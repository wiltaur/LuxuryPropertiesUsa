using LuxuryPropertiesUsa.Domain.Entities;

namespace LuxuryPropertiesUsa.Domain.Interfaces.Repositories
{
    public interface IPropertyRepository
    {
        Task<List<Property>> GetAllAsync(CancellationToken cancellationToken);
    }
}