using LuxuryPropertiesUsa.API.Controllers;
using LuxuryPropertiesUsa.Application.Features.Authentication.Queries;
using LuxuryPropertiesUsa.Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LuxuryPropertiesUsa.Test.Controllers
{
    [TestFixture]
    public class AuthenticationControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private AuthenticationController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthenticationController(_mediatorMock.Object);
        }

        [Test]
        public async Task Get_ReturnsOk_WhenResultIsSuccess()
        {
            // Arrange
            var userId = "testUser";
            var expectedResult = new ApiResponseUtility<string>(string.Empty) { IsSuccess = true };
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetTokenAuthenticateQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Get(userId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<OkObjectResult>());
                Assert.That(((OkObjectResult)result).Value, Is.EqualTo(expectedResult));
            });
        }

        [Test]
        public async Task Get_ReturnsBadRequest_WhenResultIsNotSuccess()
        {
            // Arrange
            var userId = "testUser";
            var expectedResult = new ApiResponseUtility<string>(string.Empty) { IsSuccess = false };
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetTokenAuthenticateQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Get(userId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
                Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo(expectedResult));
            });
        }
    }
}