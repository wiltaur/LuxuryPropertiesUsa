using Microsoft.EntityFrameworkCore;
using LuxuryPropertiesUsa.Domain.Entities;
using LuxuryPropertiesUsa.Infrastructure.Data;
using LuxuryPropertiesUsa.Infrastructure.Repositories;

namespace LuxuryPropertiesUsa.Test.Repositories;

[TestFixture]
public class PropertyRepositoryTests
{
    private AppDbContext _context;
    private PropertyRepository _repository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = new AppDbContext(options);
        _repository = new PropertyRepository(_context);

        // Seed Owner for navigation property
        var owner = new Owner { IdOwner = 1, Name = "John Doe", Address = "123 Main St" };
        _context.Owners.Add(owner);
        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddAsync_AddsPropertyToDatabase()
    {
        var property = new Property
        {
            IdProperty = 1,
            Name = "Luxury Villa",
            Address = "456 Ocean Drive",
            Price = 1000000,
            IdOwner = 1,
            IdOwnerNavigation = _context.Owners.Find(1) ?? throw new Exception("Owner not found")
        };

        await _repository.AddAsync(property, CancellationToken.None);
        await _context.SaveChangesAsync();

        var result = await _context.Properties.FindAsync(1);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Luxury Villa"));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsProperty_WhenExists()
    {
        var property = new Property
        {
            IdProperty = 2,
            Name = "Penthouse",
            Address = "789 Skyline Ave",
            Price = 2000000,
            IdOwner = 1,
            IdOwnerNavigation = _context.Owners.Find(1) ?? throw new Exception("Owner not found")
        };
        _context.Properties.Add(property);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(2, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Penthouse"));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        var result = await _repository.GetByIdAsync(999, CancellationToken.None);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetAllFilteredAsync_ReturnsFilteredAndSortedProperties()
    {
        _context.Properties.AddRange(
            new Property { IdProperty = 3, Name = "A House", Address = "A St", Price = 500000, IdOwner = 1, IdOwnerNavigation = _context.Owners.Find(1) ?? throw new Exception("Owner not found") },
            new Property { IdProperty = 4, Name = "B House", Address = "B St", Price = 600000, IdOwner = 1, IdOwnerNavigation = _context.Owners.Find(1) ?? throw new Exception("Owner not found") }
        );
        await _context.SaveChangesAsync();

        var resultAsc = await _repository.GetAllFilteredAsync(false, "House", 1, 10, CancellationToken.None);
        Assert.That(resultAsc.Count, Is.EqualTo(2));
        Assert.That(resultAsc[0].Name, Is.EqualTo("A House"));

        var resultDesc = await _repository.GetAllFilteredAsync(true, "House", 1, 10, CancellationToken.None);
        Assert.That(resultDesc.Count, Is.EqualTo(2));
        Assert.That(resultDesc[0].Name, Is.EqualTo("B House"));
    }

    [Test]
    public async Task GetAllFilteredAsync_ReturnsPagedResults()
    {
        for (int i = 1; i <= 2; i++)
        {
            _context.Properties.Add(new Property
            {
                IdProperty = i,
                Name = $"Property {i}",
                Address = $"Address {i}",
                Price = 100000 + i,
                IdOwner = 1,
                IdOwnerNavigation = _context.Owners.Find(1) ?? throw new Exception("Owner not found")
            });
        }
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllFilteredAsync(false, "", 2, 1, CancellationToken.None);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Property 2"));
    }

    [Test]
    public async Task GetTotalRecordsAsync_ReturnsCountWithSearchString()
    {
        _context.Properties.AddRange(
            new Property { IdProperty = 10, Name = "Alpha", Address = "Alpha St", Price = 100000, IdOwner = 1, IdOwnerNavigation = _context.Owners.Find(1) ?? throw new Exception("Owner not found") },
            new Property { IdProperty = 11, Name = "Beta", Address = "Beta St", Price = 200000, IdOwner = 1, IdOwnerNavigation = _context.Owners.Find(1) ?? throw new Exception("Owner not found") }
        );
        await _context.SaveChangesAsync();

        var count = await _repository.GetTotalRecordsAsync("Alpha", CancellationToken.None);
        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetTotalRecordsAsync_ReturnsTotalCountWithoutSearchString()
    {
        _context.Properties.AddRange(
            new Property { IdProperty = 12, Name = "Gamma", Address = "Gamma St", Price = 300000, IdOwner = 1, IdOwnerNavigation = _context.Owners.Find(1) ?? throw new Exception("Owner not found") },
            new Property { IdProperty = 13, Name = "Delta", Address = "Delta St", Price = 400000, IdOwner = 1, IdOwnerNavigation = _context.Owners.Find(1) ?? throw new Exception("Owner not found") }
        );
        await _context.SaveChangesAsync();

        var count = await _repository.GetTotalRecordsAsync("", CancellationToken.None);
        Assert.That(count, Is.EqualTo(2));
    }
}