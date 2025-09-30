using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;
using LuxuryPropertiesUsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LuxuryPropertiesUsa.Infrastructure.Repositories;
public class PropertyRepository(AppDbContext context) : IPropertyRepository
{
    public async Task<List<Property>> GetAllAsync(CancellationToken cancellationToken)
        => await context.Properties.ToListAsync(cancellationToken);
}