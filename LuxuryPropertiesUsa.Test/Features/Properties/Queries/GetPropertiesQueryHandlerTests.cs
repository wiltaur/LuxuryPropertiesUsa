using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Features.Properties.Queries;
using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Domain.Interfaces;
using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LuxuryPropertiesUsa.Test.Features.Properties.Queries;

[TestFixture]
public class GetPropertiesQueryHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IPropertyRepository> _propertyRepoMock;
    private Mock<IConfiguration> _configMock;
    private GetPropertiesQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _propertyRepoMock = new Mock<IPropertyRepository>();
        _configMock = new Mock<IConfiguration>();

        _unitOfWorkMock.Setup(u => u.Properties).Returns(_propertyRepoMock.Object);

        var defaultPageSizeSection = new Mock<IConfigurationSection>();
        defaultPageSizeSection.Setup(s => s.Value).Returns("10");
        var defaultParamsSection = new Mock<IConfigurationSection>();
        defaultParamsSection.Setup(s => s.GetSection("PageSize")).Returns(defaultPageSizeSection.Object);
        _configMock.Setup(c => c.GetSection("DefaultParams")).Returns(defaultParamsSection.Object);

        _handler = new GetPropertiesQueryHandler(_unitOfWorkMock.Object, _configMock.Object);
    }

    [Test]
    public async Task Handle_ReturnsSuccessResponse_WhenPropertiesFound()
    {
        // Arrange
        var owner = new Owner { IdOwner = 1, Name = "John Doe" };
        var properties = new List<Property>
        {
            new() {
                IdProperty = 1,
                Name = "Luxury Villa",
                Address = "123 Palm St",
                Price = 1000000,
                CodeInternal = 101,
                Year = 2020,
                IdOwner = 1,
                IdOwnerNavigation = owner
            }
        };

        _propertyRepoMock.Setup(r => r.GetAllFilteredAsync(true, "search", 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(properties);
        _propertyRepoMock.Setup(r => r.GetTotalRecordsAsync("search", It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var requestDto = new PropertyDetailRequestDto
        {
            SortOrderDesc = true,
            SearchString = "search",
            PageNumber = 1,
            PageSize = 10
        };
        var query = new GetPropertiesQuery(requestDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.ReturnMessage, Is.EqualTo("The information has been successfully generated."));
            Assert.That(result.Data, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(result.Data.TotalRecords, Is.EqualTo(1));
            Assert.That(result.Data.TotalPages, Is.EqualTo(1));
            Assert.That(result.Data.Properties, Has.Count.EqualTo(1));
        });
        Assert.Multiple(() =>
        {
            Assert.That(result.Data.Properties.First().Name, Is.EqualTo("Luxury Villa"));
            Assert.That(result.Data.Properties.First().NameOwner, Is.EqualTo("John Doe"));
        });
    }

    [Test]
    public async Task Handle_ReturnsErrorResponse_WhenExceptionThrown()
    {
        // Arrange
        _propertyRepoMock.Setup(r => r.GetAllFilteredAsync(It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        var requestDto = new PropertyDetailRequestDto
        {
            SortOrderDesc = true,
            SearchString = "search",
            PageNumber = 1,
            PageSize = 10
        };
        var query = new GetPropertiesQuery(requestDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ReturnMessage, Is.EqualTo("Database error"));
            Assert.That(result.Data, Is.Not.Null);
        });
    }

    [Test]
    public void MapInfoProperties_ReturnsMappedList()
    {
        // Arrange
        var owner = new Owner { IdOwner = 2, Name = "Jane Smith" };
        var properties = new List<Property>
        {
            new Property
            {
                IdProperty = 2,
                Name = "Modern Apartment",
                Address = "456 Oak Ave",
                Price = 500000,
                CodeInternal = 202,
                Year = 2022,
                IdOwner = 2,
                IdOwnerNavigation = owner
            }
        };

        // Act
        var result = InvokeMapInfoProperties(properties);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Modern Apartment"));
            Assert.That(result[0].NameOwner, Is.EqualTo("Jane Smith"));
            Assert.That(result[0].Id, Is.EqualTo(2));
        });
    }

    // Helper to invoke private static method for coverage
    private List<PropertyFilterDto> InvokeMapInfoProperties(List<Property> properties)
    {
        var method = typeof(GetPropertiesQueryHandler).GetMethod("MapInfoProperties", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        return (List<PropertyFilterDto>)method.Invoke(null, [properties]);
    }
}