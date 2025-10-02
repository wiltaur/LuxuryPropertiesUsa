using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Utilities;
using MediatR;

namespace LuxuryPropertiesUsa.Application.Features.Properties.Commands;

public record CreatePropertyImageCommand : IRequest<ApiResponseUtility<bool>>
{
    public PropertyImagesIdDto PropertyImages { get; }
    public CreatePropertyImageCommand(PropertyImagesIdDto propertyImages) => PropertyImages = propertyImages;
}