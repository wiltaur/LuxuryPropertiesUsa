using LuxuryPropertiesUsa.Domain.Interfaces.Repositories;
using LuxuryPropertiesUsa.Infrastructure;
using LuxuryPropertiesUsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LuxuryPropertiesUsa.Test;

[TestFixture]
public class UnitOfWorkTests
{
    private Mock<AppDbContext> _mockContext;
    private Mock<IPropertyRepository> _mockPropertyRepo;
    private Mock<IPropertyImageRepository> _mockPropertyImageRepo;
    private UnitOfWork _unitOfWork;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .Options;
        _mockContext = new Mock<AppDbContext>(options);
        _mockPropertyRepo = new Mock<IPropertyRepository>();
        _mockPropertyImageRepo = new Mock<IPropertyImageRepository>();
        _unitOfWork = new UnitOfWork(_mockContext.Object, _mockPropertyRepo.Object, _mockPropertyImageRepo.Object);
    }

    [Test]
    public void Properties_ShouldReturnInjectedRepository()
    {
        Assert.That(_unitOfWork.Properties, Is.SameAs(_mockPropertyRepo.Object));
    }

    [Test]
    public void PropertyImages_ShouldReturnInjectedRepository()
    {
        Assert.That(_unitOfWork.PropertyImages, Is.SameAs(_mockPropertyImageRepo.Object));
    }

    [Test]
    public async Task SaveChangesAsync_ShouldCallContextSaveChangesAsync_AndReturnResult()
    {
        var cancellationToken = new CancellationToken();
        _mockContext.Setup(c => c.SaveChangesAsync(cancellationToken)).ReturnsAsync(42);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        Assert.That(result, Is.EqualTo(42));
        _mockContext.Verify(c => c.SaveChangesAsync(cancellationToken), Times.Once);
    }
}