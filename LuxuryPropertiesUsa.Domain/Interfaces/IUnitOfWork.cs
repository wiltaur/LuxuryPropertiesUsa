using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;

namespace LuxuryPropertiesUsa.Domain.Interfaces;

public interface IUnitOfWork
{
    IPropertyRepository Properties { get; }
    IPropertyImageRepository PropertyImages { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}