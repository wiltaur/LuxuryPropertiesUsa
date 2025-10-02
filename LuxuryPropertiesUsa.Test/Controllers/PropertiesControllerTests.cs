using LuxuryPropertiesUsa.API.Controllers;
using LuxuryPropertiesUsa.Application.DTOs.Properties;
using LuxuryPropertiesUsa.Application.Features.Properties.Commands;
using LuxuryPropertiesUsa.Application.Features.Properties.Queries;
using LuxuryPropertiesUsa.Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LuxuryPropertiesUsa.Test.Controllers
{
    [TestFixture]
    public class PropertiesControllerTests
    {
        private Mock<IMediator> _mediatorMock = null!;
        private PropertiesController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PropertiesController(_mediatorMock.Object);
        }

        [Test]
        public async Task Create_ReturnsOk_WhenSuccess()
        {
            var dto = new PropertyAddDto
            {
                Name = "Test",
                Address = "123 Main",
                Price = 100000,
                CodeInternal = 1,
                Year = 2020,
                IdOwner = 1,
                Images = []
            };
            var expectedResult = new ApiResponseUtility<string>(string.Empty) { IsSuccess = true };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreatePropertyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _controller.Create(dto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
                Assert.That(((OkObjectResult)result).Value, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenFailure()
        {
            var dto = new PropertyAddDto();
            var expectedResult = new ApiResponseUtility<string>(string.Empty) { IsSuccess = false };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreatePropertyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _controller.Create(dto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task CreateImages_ReturnsOk_WhenSuccess()
        {
            var dto = new PropertyImagesIdDto
            {
                Id = 1,
                Images = []
            };
            var expectedResult = new ApiResponseUtility<bool>(true) { IsSuccess = true };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreatePropertyImageCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _controller.CreateImages(dto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
                Assert.That(((OkObjectResult)result).Value, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task CreateImages_ReturnsBadRequest_WhenFailure()
        {
            var dto = new PropertyImagesIdDto();
            var expectedResult = new ApiResponseUtility<bool>(false) { IsSuccess = false };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreatePropertyImageCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _controller.CreateImages(dto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task UpdatePrice_ReturnsOk_WhenSuccess()
        {
            var dto = new PropertyPriceDto { Id = 1, Price = 200000 };
            var expectedResult = new ApiResponseUtility<bool>(true) { IsSuccess = true };
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdatePropertyPriceCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _controller.UpdatePrice(dto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
                Assert.That(((OkObjectResult)result).Value, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task UpdatePrice_ReturnsBadRequest_WhenFailure()
        {
            var dto = new PropertyPriceDto();
            var expectedResult = new ApiResponseUtility<bool>(false) { IsSuccess = false };
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdatePropertyPriceCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _controller.UpdatePrice(dto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task Update_ReturnsOk_WhenSuccess()
        {
            var dto = new PropertyModifyDto { Id = 1, Price = 250000 };
            var expectedResult = new ApiResponseUtility<bool>(true) { IsSuccess = true };
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdatePropertyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _controller.Update(dto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
                Assert.That(((OkObjectResult)result).Value, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task Update_ReturnsBadRequest_WhenFailure()
        {
            var dto = new PropertyModifyDto();
            var expectedResult = new ApiResponseUtility<bool>(false) { IsSuccess = false };
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdatePropertyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _controller.Update(dto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task Get_ReturnsOk_WhenSuccess()
        {
            var dto = new PropertyDetailRequestDto { SearchString = "Test" };
            var dtoResp = new PropertyDetailResponseDto();
            var expectedResult = new ApiResponseUtility<PropertyDetailResponseDto>(dtoResp) { IsSuccess = true };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPropertiesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _controller.Get(dto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
                Assert.That(((OkObjectResult)result).Value, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task Get_ReturnsBadRequest_WhenFailure()
        {
            var dto = new PropertyDetailRequestDto();
            var dtoResp = new PropertyDetailResponseDto();
            var expectedResult = new ApiResponseUtility<PropertyDetailResponseDto>(dtoResp) { IsSuccess = false };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPropertiesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var result = await _controller.Get(dto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo(expectedResult));
            });
        }
    }
}