using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;

namespace LuxuryPropertiesUsa.Domain.Interfaces;

public interface IUnitOfWork
{
    IPropertyRepository Properties { get; }
    Task<int> SaveChangesAsync();
}