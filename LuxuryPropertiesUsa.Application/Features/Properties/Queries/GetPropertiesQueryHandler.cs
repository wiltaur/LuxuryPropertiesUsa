using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Domain.Interfaces;
using MediatR;

namespace LuxuryPropertiesUsa.Application.Features.Properties.Queries;

public record GetPropertiesQuery : IRequest<List<Property>>;

public class GetPropertiesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPropertiesQuery, List<Property>>
{
    public async Task<List<Property>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.Properties.GetAllAsync(cancellationToken);
    }
}