using LuxuryPropertiesUsa.Application.Utilities;
using LuxuryPropertiesUsa.Domain.Interfaces;
using MediatR;

namespace LuxuryPropertiesUsa.Application.Features.Properties.Commands;

public class UpdatePropertyCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdatePropertyCommand, ApiResponseUtility<bool>>
{
    /// <summary>
    /// Method to update info of Property in the database.
    /// </summary>
    /// <param name="request">Dto that content id and columns to update of one Property.</param>
    /// <returns>When upadated successfully, true is returned, otherwise false are returned.</returns>
    public async Task<ApiResponseUtility<bool>> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var property = await unitOfWork.Properties.GetByIdAsync(request.Property.Id, cancellationToken) 
                ?? throw new Exception("The property to be updated does not exist.");

            property.Address = request.Property.Address ?? property.Address;
            property.Name = request.Property.Name ?? property.Name;
            property.Year = request.Property.Year ?? property.Year;
            property.Price = request.Property.Price ?? property.Price;
            property.CodeInternal = request.Property.CodeInternal ?? property.CodeInternal;
            property.IdOwner = request.Property.IdOwner ?? property.IdOwner;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new ApiResponseUtility<bool>(true)
            {
                IsSuccess = true,
                ReturnMessage = "The property has been successfully updated."
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
}