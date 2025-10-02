using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Features.Properties.Commands;
using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Domain.Interfaces;
using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;
using Moq;

namespace LuxuryPropertiesUsa.Test.Features.Properties.Commands
{
    [TestFixture]
    public class CreatePropertyImageCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IPropertyImageRepository> _propertyImageRepoMock;
        private CreatePropertyImageCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _propertyImageRepoMock = new Mock<IPropertyImageRepository>();
            _unitOfWorkMock.SetupGet(u => u.PropertyImages).Returns(_propertyImageRepoMock.Object);
            _handler = new CreatePropertyImageCommandHandler(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnSuccessResponse_WhenImagesAreAdded()
        {
            // Arrange
            var propertyImagesDto = new PropertyImagesIdDto
            {
                Id = 1,
                Images =
                [
                    new() { File = Convert.ToBase64String(new byte[] { 1, 2, 3 }), Enabled = true }
                ]
            };
            var command = new CreatePropertyImageCommand(propertyImagesDto);

            _propertyImageRepoMock
                .Setup(r => r.AddRangeAsync(It.IsAny<List<PropertyImage>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

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
                Assert.That(result.ReturnMessage, Is.EqualTo("The information has been successfully saved."));
            });
        }

        [Test]
        public async Task Handle_ShouldReturnFailureResponse_WhenExceptionIsThrown()
        {
            // Arrange
            var propertyImagesDto = new PropertyImagesIdDto
            {
                Id = 1,
                Images =
                [
                    new() { File = Convert.ToBase64String(new byte[] { 1, 2, 3 }), Enabled = true }
                ]
            };
            var command = new CreatePropertyImageCommand(propertyImagesDto);

            _propertyImageRepoMock
                .Setup(r => r.AddRangeAsync(It.IsAny<List<PropertyImage>>(), It.IsAny<CancellationToken>()))
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

        [Test]
        public void MapInfoPropertyIdImage_ShouldMapImagesCorrectly()
        {
            // Arrange
            var propertyImagesDto = new PropertyImagesIdDto
            {
                Id = 5,
                Images =
                [
                    new() { File = Convert.ToBase64String(new byte[] { 10, 20 }), Enabled = true },
                    new() { File = Convert.ToBase64String(new byte[] { 30, 40 }), Enabled = false }
                ]
            };

            // Act
            var result = InvokeMapInfoPropertyIdImage(propertyImagesDto);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));
                Assert.That(result[0].IdProperty, Is.EqualTo(5));
                Assert.That(result[0].File, Is.EqualTo(new byte[] { 10, 20 }));
                Assert.That(result[0].Enabled, Is.True);
                Assert.That(result[1].IdProperty, Is.EqualTo(5));
                Assert.That(result[1].File, Is.EqualTo(new byte[] { 30, 40 }));
                Assert.That(result[1].Enabled, Is.False);
            });
        }

        // Helper to invoke private static method for testing
        private static List<PropertyImage> InvokeMapInfoPropertyIdImage(PropertyImagesIdDto dto)
        {
            var method = typeof(CreatePropertyImageCommandHandler)
                .GetMethod("MapInfoPropertyIdImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            return (List<PropertyImage>)method.Invoke(null, [dto]);
        }
    }
}