using MongoDB.Driver.GeoJsonObjectModel;
using SmartCity.Api.Models;
using SmartCity.Api.Repositories;

namespace SmartCity.Api.Services;

public class PointOfInterestService
{
    private readonly PointOfInterestRepository _pointOfInterestRepository;
    private readonly PointOfInterestCategoryRepository _pointOfInterestCategoryRepository;

    public PointOfInterestService(
        PointOfInterestRepository pointOfInterestRepository,
        PointOfInterestCategoryRepository pointOfInterestCategoryRepository)
    {
        _pointOfInterestRepository = pointOfInterestRepository;
        _pointOfInterestCategoryRepository = pointOfInterestCategoryRepository;
    }

    public async Task<List<PointOfInterest>> GetActiveAsync()
    {
        return await _pointOfInterestRepository.GetActiveAsync();
    }

    public async Task<List<PointOfInterest>> GetByCategoryIdAsync(string categoryId)
    {
        return await _pointOfInterestRepository.GetByCategoryIdAsync(categoryId);
    }

    public async Task<PointOfInterest?> GetByIdAsync(string id)
    {
        return await _pointOfInterestRepository.GetByIdAsync(id);
    }

    public async Task<PointOfInterest> CreateAsync(
        string adminUserId,
        string categoryId,
        string name,
        string description,
        double longitude,
        double latitude,
        string address,
        string? imageUrl,
        string? phoneNumber,
        string? websiteUrl,
        string? openingHours,
        DateTime? validFrom,
        DateTime? validUntil,
        bool isTemporary)
    {
        var category = await _pointOfInterestCategoryRepository.GetByIdAsync(categoryId);
        if (category is null)
        {
            throw new KeyNotFoundException("Category not found.");
        }

        if (!category.IsActive)
        {
            throw new InvalidOperationException("Category is not active.");
        }

        var location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
            new GeoJson2DGeographicCoordinates(longitude, latitude));

        var pointOfInterest = new PointOfInterest
        {
            CategoryId = categoryId,
            Name = name,
            Description = description,
            Location = location,
            Address = address,
            ImageUrl = imageUrl,
            PhoneNumber = phoneNumber,
            WebsiteUrl = websiteUrl,
            OpeningHours = openingHours,
            ValidFrom = validFrom,
            ValidUntil = validUntil,
            IsTemporary = isTemporary,
            IsActive = true,
            Audit = new Audit
            {
                InsertedBy = adminUserId,
                ModifiedBy = null,
                InsertedAt = DateTime.UtcNow,
                ModifiedAt = null
            }
        };

        return await _pointOfInterestRepository.CreateAsync(pointOfInterest);
    }
}
