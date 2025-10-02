using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Features.Properties.Commands;
using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Domain.Interfaces;
using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;
using Moq;

namespace LuxuryPropertiesUsa.Test.Features.Properties.Commands;

[TestFixture]
public class UpdatePropertyCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private Mock<IPropertyRepository> _propertyRepoMock = null!;
    private UpdatePropertyCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _propertyRepoMock = new Mock<IPropertyRepository>();
        _unitOfWorkMock.SetupGet(u => u.Properties).Returns(_propertyRepoMock.Object);
        _handler = new UpdatePropertyCommandHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_PropertyExists_UpdatesPropertyAndReturnsSuccess()
    {
        // Arrange
        var property = new Property
        {
            IdProperty = 1,
            Address = "Old Address",
            Name = "Old Name",
            Year = 2000,
            Price = 100000,
            CodeInternal = 123,
            IdOwner = 1
        };

        var modifyDto = new PropertyModifyDto
        {
            Id = 1,
            Address = "New Address",
            Name = "New Name",
            Year = 2022,
            Price = 200000,
            CodeInternal = 456,
            IdOwner = 2
        };

        _propertyRepoMock
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(property);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var command = new UpdatePropertyCommand(modifyDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.True);
            Assert.That(result.ReturnMessage, Is.EqualTo("The property has been successfully updated."));
            Assert.That(property.Address, Is.EqualTo("New Address"));
            Assert.That(property.Name, Is.EqualTo("New Name"));
            Assert.That(property.Year, Is.EqualTo(2022));
            Assert.That(property.Price, Is.EqualTo(200000));
            Assert.That(property.CodeInternal, Is.EqualTo(456));
            Assert.That(property.IdOwner, Is.EqualTo(2));
        });
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_PropertyDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var modifyDto = new PropertyModifyDto { Id = 99 };
        _propertyRepoMock
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property?)null);

        var command = new UpdatePropertyCommand(modifyDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Data, Is.False);
            Assert.That(result.ReturnMessage, Is.EqualTo("The property to be updated does not exist."));
        });
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}