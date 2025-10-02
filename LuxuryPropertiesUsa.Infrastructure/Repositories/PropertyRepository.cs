using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;
using LuxuryPropertiesUsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LuxuryPropertiesUsa.Infrastructure.Repositories;
public class PropertyRepository(AppDbContext context) : IPropertyRepository
{
    public async Task AddAsync(Property property, CancellationToken cancellationToken)
        => await context.Properties.AddAsync(property, cancellationToken);

    public async Task<Property?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.Properties.FindAsync([id], cancellationToken);

    public async Task<List<Property>> GetAllFilteredAsync(bool sortOrderDesc, string searchString, int pageNumber, int pageSize, CancellationToken cancellationToken)

         => await (from prop in context.Properties.Include(c => c.IdOwnerNavigation)
                   orderby
                     !sortOrderDesc ? prop.Name : "",
                     !sortOrderDesc ? "" : prop.Name descending
                   where !string.IsNullOrEmpty(searchString) ? (prop.Name.Contains(searchString)
                    || prop.IdOwnerNavigation.Name.Contains(searchString)
                    || prop.Address.Contains(searchString)) : 0 == 0
                   select prop)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

    public async Task<int> GetTotalRecordsAsync(string searchString, CancellationToken cancellationToken)
    {
        return !string.IsNullOrEmpty(searchString)
            ? await context.Properties
                .Include(c => c.IdOwnerNavigation)
                .Where(p => p.Name.Contains(searchString)
                    || p.IdOwnerNavigation.Name.Contains(searchString)
                    || p.Address.Contains(searchString))
                .CountAsync(cancellationToken)
            : await context.Properties.CountAsync(cancellationToken);
    }
}