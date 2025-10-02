using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Utilities;
using MediatR;

namespace LuxuryPropertiesUsa.Application.Features.Properties.Commands;

public record UpdatePropertyPriceCommand : IRequest<ApiResponseUtility<bool>>
{
    public PropertyPriceDto PropertyPrice { get; }
    public UpdatePropertyPriceCommand(PropertyPriceDto propertyPrice) => PropertyPrice = propertyPrice;
}