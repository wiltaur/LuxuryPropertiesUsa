using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Utilities;
using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Domain.Interfaces;
using MediatR;

namespace LuxuryPropertiesUsa.Application.Features.Properties.Commands;

public class CreatePropertyImageCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreatePropertyImageCommand, ApiResponseUtility<bool>>
{
    /// <summary>
    /// Public method to add a list of images from property with id to the database.
    /// </summary>
    /// <param name="request">Dto that content all information for images of Property incluning the property id.</param>
    /// <returns>When added successfully, true is returned, otherwise false is returned.</returns>
    public async Task<ApiResponseUtility<bool>> Handle(CreatePropertyImageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var lstPImages = MapInfoPropertyIdImage(request.PropertyImages);
            await unitOfWork.PropertyImages.AddRangeAsync(lstPImages, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new ApiResponseUtility<bool>(true)
            {
                IsSuccess = true,
                ReturnMessage = "The information has been successfully saved."
            };
        }
        catch (Exception ex)
        {
            var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            return new ApiResponseUtility<bool>(false)
            {
                IsSuccess = false,
                ReturnMessage = message
            };
        }
    }

    /// <summary>
    /// Private method to map all images of one property with id.
    /// </summary>
    /// <param name="propertyImages">Dto with Property id and images information.</param>
    /// <returns>List of images with property id mapped.</returns>
    private static List<PropertyImage> MapInfoPropertyIdImage(PropertyImagesIdDto propertyImages)
    {
        List<PropertyImage> lstPImages = new();
        foreach (var image in propertyImages.Images)
        {
            PropertyImage propertyImage = new()
            {
                IdProperty = propertyImages.Id,
                File = Convert.FromBase64String(image.File),
                Enabled = image.Enabled
            };
            lstPImages.Add(propertyImage);
        }
        return lstPImages;
    }
}