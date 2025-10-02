using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Features.Properties.Commands;
using LuxuryPropertiesUsa.Application.Features.Properties.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxuryPropertiesUsa.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PropertiesController(IMediator mediator) : ControllerBase
{
    #region MainMethods

    /// <summary>
    /// Add a property and its images.
    /// </summary>
    /// <param name="property">Object that content all information for Property incluning the images list.</param>
    /// <returns>When added successfully, true and Ok are returned, otherwise false and BadRequest are returned.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PropertyAddDto property)
    {
        var command = new CreatePropertyCommand(property);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Add a list of images from property with id.
    /// </summary>
    /// <param name="propertyImages">Object that content all information for images of Property incluning the property id.</param>
    /// <returns>When added successfully, true and Ok are returned, otherwise false and BadRequest are returned.</returns>
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateImages([FromBody] PropertyImagesIdDto propertyImages)
    {
        var command = new CreatePropertyImageCommand(propertyImages);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Change the price of Property.
    /// </summary>
    /// <param name="propertyPrice">Object that content id and new price of Property.</param>
    /// <returns>When updated successfully, true and Ok are returned, otherwise false and BadRequest are returned.</returns>
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdatePrice([FromBody] PropertyPriceDto propertyPrice)
    {
        var command = new UpdatePropertyPriceCommand(propertyPrice);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Update fields of Property.
    /// </summary>
    /// <param name="property">Object that content id and other fields to update of Property.</param>
    /// <returns>When updated successfully, true and Ok are returned, otherwise false and BadRequest are returned.</returns>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PropertyModifyDto property)
    {
        var command = new UpdatePropertyCommand(property);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Search all properties that match the filters.
    /// </summary>
    /// <remarks>
    /// Parameters description:
    /// 
    ///      searchString => Field for the filter that matches the property name, address, and owner name.
    ///      sortOrderDesc => true for sort descending, default is ascending.
    ///      currentFilter => Field for admin the searchString when navigated in the pages of the table.
    ///      pageNumber => It is the actual page number of the table.
    ///      pageSize => It is the maximum number of items per page
    /// </remarks>
    /// <param name="property">Object that content the filters for print on tables.</param>
    /// <returns>When search is successfully, List of Properties with Owner information and Ok are returned, 
    /// otherwise BadRequest are returned.</returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PropertyDetailRequestDto property)
    {
        var command = new GetPropertiesQuery(property);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    #endregion MainMethods
}