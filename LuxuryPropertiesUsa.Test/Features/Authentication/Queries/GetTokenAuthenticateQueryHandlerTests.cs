using LuxuryPropertiesUsa.Application.Features.Authentication.Queries;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LuxuryPropertiesUsa.Test.Features.Authentication.Queries
{
    [TestFixture]
    public class GetTokenAuthenticateQueryHandlerTests
    {
        private Mock<IConfiguration> _configMock;
        private GetTokenAuthenticateQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _configMock = new Mock<IConfiguration>();

            // Setup configuration sections
            var sections = new Dictionary<string, string>
            {
                { "SecretsValues:AuthKey", "supersecretkey1234567890qwerty12" },
                { "SecretsValues:AuthIssuer", "TestIssuer" },
                { "SecretsValues:AuthAudience", "TestAudience" },
                { "SecretsValues:MinExpire", "10" }
            };

            foreach (var kvp in sections)
            {
                var sectionMock = new Mock<IConfigurationSection>();
                sectionMock.Setup(s => s.Value).Returns(kvp.Value);
                _configMock.Setup(c => c.GetSection(kvp.Key)).Returns(sectionMock.Object);
            }

            _handler = new GetTokenAuthenticateQueryHandler(_configMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsSuccessResponseWithToken()
        {
            // Arrange
            var query = new GetTokenAuthenticateQuery("user123");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Data, Is.Not.Null);
                Assert.That(result.ReturnMessage, Is.EqualTo("The token has been successfully generated."));
            });
            Assert.That(result.Data, Is.Not.Empty);
        }

        [Test]
        public async Task Handle_MissingAuthKey_ReturnsErrorResponse()
        {
            // Arrange
            var sectionMock = new Mock<IConfigurationSection>();
            string? returnedValue = null;
            sectionMock.Setup(s => s.Value).Returns(returnedValue);
            _configMock.Setup(c => c.GetSection("SecretsValues:AuthKey")).Returns(sectionMock.Object);

            var handler = new GetTokenAuthenticateQueryHandler(_configMock.Object);
            var query = new GetTokenAuthenticateQuery("user123");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.ReturnMessage, Is.EqualTo("Error getting token."));
                Assert.That(result.Data, Is.Not.Null);
            });
        }

        [Test]
        public async Task Handle_InvalidMinExpire_UsesDefaultValue()
        {
            // Arrange
            var sectionMock = new Mock<IConfigurationSection>();
            sectionMock.Setup(s => s.Value).Returns("notanumber");
            _configMock.Setup(c => c.GetSection("SecretsValues:MinExpire")).Returns(sectionMock.Object);

            var handler = new GetTokenAuthenticateQueryHandler(_configMock.Object);
            var query = new GetTokenAuthenticateQuery("user123");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.ReturnMessage, Is.EqualTo("Error getting token."));
                Assert.That(result.Data, Is.Not.Null);
            });
        }
    }
}