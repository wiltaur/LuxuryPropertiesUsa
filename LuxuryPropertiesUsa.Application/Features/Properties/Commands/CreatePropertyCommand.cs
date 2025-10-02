using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Utilities;
using MediatR;

namespace LuxuryPropertiesUsa.Application.Features.Properties.Commands;

public record CreatePropertyCommand : IRequest<ApiResponseUtility<string>>
{
    public PropertyAddDto PropertyAdd { get; }
    public CreatePropertyCommand(PropertyAddDto propertyAdd) => PropertyAdd = propertyAdd;
}