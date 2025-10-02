using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Utilities;
using MediatR;

namespace LuxuryPropertiesUsa.Application.Features.Properties.Commands;

public record UpdatePropertyCommand : IRequest<ApiResponseUtility<bool>>
{
    public PropertyModifyDto Property { get; }
    public UpdatePropertyCommand(PropertyModifyDto property) => Property = property;
}