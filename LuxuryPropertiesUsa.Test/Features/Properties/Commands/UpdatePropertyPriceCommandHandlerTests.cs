using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Features.Properties.Commands;
using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Domain.Interfaces;
using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;
using Moq;

namespace LuxuryPropertiesUsa.Test.Features.Properties.Commands
{
    [TestFixture]
    public class UpdatePropertyPriceCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock = null!;
        private Mock<IPropertyRepository> _propertyRepositoryMock = null!;
        private UpdatePropertyPriceCommandHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _propertyRepositoryMock = new Mock<IPropertyRepository>();
            _unitOfWorkMock.SetupGet(u => u.Properties).Returns(_propertyRepositoryMock.Object);
            _handler = new UpdatePropertyPriceCommandHandler(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task Handle_ShouldUpdatePrice_WhenPropertyExists()
        {
            // Arrange
            Property? property = new() { IdProperty = 1, Price = 100000m };
            var dto = new PropertyPriceDto { Id = 1, Price = 120000m };
            var command = new UpdatePropertyPriceCommand(dto);

                _propertyRepositoryMock
                .Setup(r => r.GetByIdAsync(dto.Id, default))
                .ReturnsAsync(property);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Data, Is.True);
                Assert.That(result.ReturnMessage, Is.EqualTo("The price has been successfully updated."));
                Assert.That(property.Price, Is.EqualTo(dto.Price));
            });
        }

        [Test]
        public async Task Handle_ShouldReturnFalse_WhenPropertyDoesNotExist()
        {
            // Arrange
            var dto = new PropertyPriceDto { Id = 99, Price = 500000m };
            var command = new UpdatePropertyPriceCommand(dto);

            _propertyRepositoryMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Property?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Data, Is.False);
                Assert.That(result.ReturnMessage, Is.EqualTo("The property to be updated does not exist."));
            });
        }

        [Test]
        public async Task Handle_ShouldReturnFalse_WhenExceptionThrown()
        {
            // Arrange
            var dto = new PropertyPriceDto { Id = 2, Price = 200000m };
            var command = new UpdatePropertyPriceCommand(dto);

            _propertyRepositoryMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Data, Is.False);
                Assert.That(result.ReturnMessage, Is.EqualTo("Database error"));
            });
        }
    }
}