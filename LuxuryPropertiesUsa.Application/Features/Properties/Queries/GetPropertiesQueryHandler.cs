using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Utilities;
using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace LuxuryPropertiesUsa.Application.Features.Properties.Queries;

public class GetPropertiesQueryHandler(IUnitOfWork unitOfWork, IConfiguration config) : IRequestHandler<GetPropertiesQuery, ApiResponseUtility<PropertyDetailResponseDto>>
{
    /// <summary>
    /// This is a method that receives a search filter and is matched against the property name, address, and owner's name. 
    /// It also receives the order to respond information and the current page and number of items per page.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>List of Properties with Owner information.</returns>
    public async Task<ApiResponseUtility<PropertyDetailResponseDto>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
    {
        PropertyDetailResponseDto propertiesResponse = new()
        {
            SortOrderDesc = request.PropertyDetail.SortOrderDesc
        };
        request.PropertyDetail.SearchString ??= request.PropertyDetail.CurrentFilter;
        propertiesResponse.CurrentFilter = request.PropertyDetail.SearchString;

        try
        {
            int pageNumber = request.PropertyDetail.PageNumber ?? 1;
            int pageSize = request.PropertyDetail.PageSize ?? 
                Convert.ToInt32(config.GetSection("DefaultParams").GetSection("PageSize").Value);

            var properties = await unitOfWork.Properties.GetAllFilteredAsync(propertiesResponse.SortOrderDesc, request.PropertyDetail.SearchString, pageNumber, pageSize, cancellationToken);

            List<PropertyFilterDto> propertiesMap = properties != null ? MapInfoProperties(properties) : [];

            propertiesResponse.TotalRecords = await unitOfWork.Properties.GetTotalRecordsAsync(request.PropertyDetail.SearchString, cancellationToken);
            propertiesResponse.TotalPages = (int)Math.Ceiling((double)propertiesResponse.TotalRecords / pageSize);
            propertiesResponse.PageNumber = pageNumber;
            propertiesResponse.PageSize = pageSize;
            propertiesResponse.Properties = propertiesMap;

            return new ApiResponseUtility<PropertyDetailResponseDto>(propertiesResponse)
            {
                IsSuccess = true,
                ReturnMessage = "The information has been successfully generated."
            };
        }
        catch (Exception ex)
        {
            var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            return new ApiResponseUtility<PropertyDetailResponseDto>(propertiesResponse)
            {
                IsSuccess = false,
                ReturnMessage = message
            };
        }
    }

    /// <summary>
    /// Private method to map all properties filtered.
    /// </summary>
    /// <param name="properties">List with Property information.</param>
    /// <returns>List of properties mapped.</returns>
    private static List<PropertyFilterDto> MapInfoProperties(List<Property> properties)
    {
        List<PropertyFilterDto> propertiesFilter = [];
        foreach (var proper in properties)
        {
            PropertyFilterDto propertyFilter = new()
            {
                Id = proper.IdProperty,
                Address = proper.Address,
                CodeInternal = proper.CodeInternal,
                IdOwner = proper.IdOwner,
                Name = proper.Name,
                NameOwner = proper.IdOwnerNavigation.Name,
                Price = proper.Price,
                Year = proper.Year
            };
            propertiesFilter.Add(propertyFilter);
        }
        return propertiesFilter;
    }
}