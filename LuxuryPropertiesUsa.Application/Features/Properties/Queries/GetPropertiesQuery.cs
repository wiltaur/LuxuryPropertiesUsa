using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Utilities;
using MediatR;

namespace LuxuryPropertiesUsa.Application.Features.Properties.Queries;

public record GetPropertiesQuery : IRequest<ApiResponseUtility<PropertyDetailResponseDto>>
{
    public PropertyDetailRequestDto PropertyDetail { get; }
    public GetPropertiesQuery(PropertyDetailRequestDto propertyDetail) => PropertyDetail = propertyDetail;
}