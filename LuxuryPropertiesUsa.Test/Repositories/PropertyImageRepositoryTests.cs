using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Infrastructure.Data;
using LuxuryPropertiesUsa.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LuxuryPropertiesUsa.Test.Repositories;

[TestFixture]
public class PropertyImageRepositoryTests
{
    private Mock<AppDbContext> _mockContext;
    private Mock<DbSet<PropertyImage>> _mockDbSet;
    private PropertyImageRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        _mockDbSet = new Mock<DbSet<PropertyImage>>();
        _mockContext.Setup(c => c.PropertyImages).Returns(_mockDbSet.Object);
        _repository = new PropertyImageRepository(_mockContext.Object);
    }

    [Test]
    public async Task AddRangeAsync_CallsAddRangeAsyncOnDbSet()
    {
        // Arrange
        var images = new List<PropertyImage>
        {
            new() { IdPropertyImage = 1, IdProperty = 1, File = new byte[] { 1 }, Enabled = true },
            new() { IdPropertyImage = 2, IdProperty = 2, File = new byte[] { 2 }, Enabled = false }
        };
        var cancellationToken = CancellationToken.None;

        _mockDbSet
            .Setup(d => d.AddRangeAsync(images, cancellationToken))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _repository.AddRangeAsync(images, cancellationToken);

        // Assert
        _mockDbSet.Verify(d => d.AddRangeAsync(images, cancellationToken), Times.Once);
    }
}