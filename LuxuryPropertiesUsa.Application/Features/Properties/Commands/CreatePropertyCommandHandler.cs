using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Utilities;
using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Domain.Interfaces;
using MediatR;

namespace LuxuryPropertiesUsa.Application.Features.Properties.Commands;

public class CreatePropertyCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreatePropertyCommand, ApiResponseUtility<string>>
{
    /// <summary>
    /// Public method to add a property and its images to the database.
    /// </summary>
    /// <param name="request">Dto that content all information for Property incluning the images list.</param>
    /// <returns>When added successfully, true is returned, otherwise false is returned.</returns>
    public async Task<ApiResponseUtility<string>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Property newProperty = MapInfoProperty(request.PropertyAdd);

            await unitOfWork.Properties.AddAsync(newProperty, cancellationToken);

            if (request.PropertyAdd.Images.Count != 0)
            {
                var propertyImages = MapInfoPropertyImage(request.PropertyAdd, newProperty);

                await unitOfWork.PropertyImages.AddRangeAsync(propertyImages, cancellationToken);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new ApiResponseUtility<string>(newProperty.Name)
            {
                IsSuccess = true,
                ReturnMessage = "The information has been successfully saved."
            };
        }
        catch (Exception ex)
        {
            var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            return new ApiResponseUtility<string>(message)
            {
                IsSuccess = false,
                ReturnMessage = "Error processing the information."
            };
        }
    }

    /// <summary>
    /// Private method to map Property information.
    /// </summary>
    /// <param name="property">Dto with Property information.</param>
    /// <returns>Property mapped.</returns>
    private static Property MapInfoProperty(PropertyAddDto property)
    {
        Property propertyInfo = new()
        {
            Name = property.Name,
            Address = property.Address,
            Price = property.Price,
            CodeInternal = property.CodeInternal,
            Year = property.Year,
            IdOwner = property.IdOwner
        };
        return propertyInfo;
    }

    /// <summary>
    /// Private method to map all images of the property.
    /// </summary>
    /// <param name="property">Dto with Property information.</param>
    /// <param name="newProperty">Property to add to the dababase.</param>
    /// <returns>List of images mapped.</returns>
    private static List<PropertyImage> MapInfoPropertyImage(PropertyAddDto property, Property newProperty)
    {
        List<PropertyImage> lstPImages = [];
        foreach (var image in property.Images)
        {
            PropertyImage propertyImage = new()
            {
                IdPropertyNavigation = newProperty,
                File = Convert.FromBase64String(image.File),
                Enabled = image.Enabled
            };
            lstPImages.Add(propertyImage);
        }
        return lstPImages;
    }
}