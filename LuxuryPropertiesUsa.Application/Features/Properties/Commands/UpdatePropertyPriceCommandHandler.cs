using LuxuryPropertiesUsa.Application.Utilities;
using LuxuryPropertiesUsa.Domain.Interfaces;
using MediatR;

namespace LuxuryPropertiesUsa.Application.Features.Properties.Commands;

public class UpdatePropertyPriceCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdatePropertyPriceCommand, ApiResponseUtility<bool>>
{
    /// <summary>
    /// Method to change the price of Property in the database.
    /// </summary>
    /// <param name="request">Dto that content id and new price of one Property.</param>
    /// <returns>When upadated successfully, true is returned, otherwise false are returned.</returns>
    public async Task<ApiResponseUtility<bool>> Handle(UpdatePropertyPriceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var property = await unitOfWork.Properties.GetByIdAsync(request.PropertyPrice.Id, cancellationToken) 
                ?? throw new Exception("The property to be updated does not exist.");

            property.Price = request.PropertyPrice.Price;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new ApiResponseUtility<bool>(true)
            {
                IsSuccess = true,
                ReturnMessage = "The price has been successfully updated."
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