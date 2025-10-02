using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Features.Properties.Commands;
using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Domain.Interfaces;
using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;
using Moq;
using System.Reflection;

namespace LuxuryPropertiesUsa.Test.Features.Properties.Commands
{
    [TestFixture]
    public class CreatePropertyCommandHandlerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IPropertyRepository> _propertyRepoMock;
        private Mock<IPropertyImageRepository> _propertyImageRepoMock;
        private CreatePropertyCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _propertyRepoMock = new Mock<IPropertyRepository>();
            _propertyImageRepoMock = new Mock<IPropertyImageRepository>();

            _unitOfWorkMock.SetupGet(u => u.Properties).Returns(_propertyRepoMock.Object);
            _unitOfWorkMock.SetupGet(u => u.PropertyImages).Returns(_propertyImageRepoMock.Object);

            _handler = new CreatePropertyCommandHandler(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task Handle_PropertyWithImages_SuccessfulSave_ReturnsSuccessResponse()
        {
            // Arrange
            var propertyAddDto = new PropertyAddDto
            {
                Name = "Test Property",
                Address = "123 Main St",
                Price = 1000000,
                CodeInternal = 123,
                Year = 2022,
                IdOwner = 1,
                Images =
                [
                    new PropertyImageDto { File = Convert.ToBase64String(new byte[] { 1, 2, 3 }), Enabled = true }
                ]
            };
            var command = new CreatePropertyCommand(propertyAddDto);

            _propertyRepoMock.Setup(r => r.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _propertyImageRepoMock.Setup(r => r.AddRangeAsync(It.IsAny<List<PropertyImage>>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Data, Is.EqualTo(propertyAddDto.Name));
                Assert.That(result.ReturnMessage, Is.EqualTo("The information has been successfully saved."));
            });
        }

        [Test]
        public async Task Handle_PropertyWithoutImages_SuccessfulSave_ReturnsSuccessResponse()
        {
            // Arrange
            var propertyAddDto = new PropertyAddDto
            {
                Name = "No Image Property",
                Address = "456 Main St",
                Price = 500000,
                CodeInternal = 456,
                Year = 2023,
                IdOwner = 2,
                Images = new List<PropertyImageDto>()
            };
            var command = new CreatePropertyCommand(propertyAddDto);

            _propertyRepoMock.Setup(r => r.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Data, Is.EqualTo(propertyAddDto.Name));
                Assert.That(result.ReturnMessage, Is.EqualTo("The information has been successfully saved."));
            });
        }

        [Test]
        public async Task Handle_ExceptionThrown_ReturnsErrorResponse()
        {
            // Arrange
            var propertyAddDto = new PropertyAddDto
            {
                Name = "Error Property",
                Address = "789 Main St",
                Price = 750000,
                CodeInternal = 789,
                Year = 2024,
                IdOwner = 3,
                Images = []
            };
            var command = new CreatePropertyCommand(propertyAddDto);

            _propertyRepoMock.Setup(r => r.AddAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Data, Is.EqualTo("Database error"));
                Assert.That(result.ReturnMessage, Is.EqualTo("Error processing the information."));
            });
        }

        [Test]
        public void MapInfoProperty_MapsDtoToProperty_Correctly()
        {
            // Arrange
            var propertyAddDto = new PropertyAddDto
            {
                Name = "MapTest",
                Address = "Map Address",
                Price = 123456,
                CodeInternal = 321,
                Year = 2025,
                IdOwner = 99,
                Images = new List<PropertyImageDto>()
            };

            // Use reflection to invoke private static method
            var method = typeof(CreatePropertyCommandHandler)
                .GetMethod("MapInfoProperty", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.That(method, Is.Not.Null);

            var property = (Property)method.Invoke(null, [propertyAddDto]);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(property?.Name, Is.EqualTo(propertyAddDto.Name));
                Assert.That(property?.Address, Is.EqualTo(propertyAddDto.Address));
                Assert.That(property?.Price, Is.EqualTo(propertyAddDto.Price));
                Assert.That(property?.CodeInternal, Is.EqualTo(propertyAddDto.CodeInternal));
                Assert.That(property?.Year, Is.EqualTo(propertyAddDto.Year));
                Assert.That(property?.IdOwner, Is.EqualTo(propertyAddDto.IdOwner));
            });
        }

        [Test]
        public void MapInfoPropertyImage_MapsImages_Correctly()
        {
            // Arrange
            var propertyAddDto = new PropertyAddDto
            {
                Name = "ImageTest",
                Address = "Image Address",
                Price = 654321,
                CodeInternal = 123,
                Year = 2026,
                IdOwner = 88,
                Images =
                [
                    new PropertyImageDto { File = Convert.ToBase64String(new byte[] { 10, 20, 30 }), Enabled = true },
                    new PropertyImageDto { File = Convert.ToBase64String(new byte[] { 40, 50, 60 }), Enabled = false }
                ]
            };
            var newProperty = new Property { Name = propertyAddDto.Name };

            // Use reflection to invoke private static method
            var method = typeof(CreatePropertyCommandHandler)
                .GetMethod("MapInfoPropertyImage", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.That(method, Is.Not.Null);

            var images = (List<PropertyImage>)method.Invoke(null, [propertyAddDto, newProperty]);

            // Assert
            Assert.That(images, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(images[0].File, Is.EqualTo(new byte[] { 10, 20, 30 }));
                Assert.That(images[0].Enabled, Is.True);
                Assert.That(images[0].IdPropertyNavigation, Is.EqualTo(newProperty));
                Assert.That(images[1].File, Is.EqualTo(new byte[] { 40, 50, 60 }));
                Assert.That(images[1].Enabled, Is.False);
                Assert.That(images[1].IdPropertyNavigation, Is.EqualTo(newProperty));
            });
        }
    }
}