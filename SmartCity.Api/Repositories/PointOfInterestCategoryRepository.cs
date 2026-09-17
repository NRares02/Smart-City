using MongoDB.Driver;
using SmartCity.Api.Models;

namespace SmartCity.Api.Repositories;

public class PointOfInterestCategoryRepository
{
    private readonly IMongoCollection<PointOfInterestCategory> _categories;

    public PointOfInterestCategoryRepository(IMongoDatabase database)
    {
        _categories = database.GetCollection<PointOfInterestCategory>("point_of_interest_categories");
    }

    public async Task<List<PointOfInterestCategory>> GetActiveAsync()
    {
        return await _categories.Find(c => c.IsActive == true)
            .SortBy(c => c.DisplayOrder)
            .ToListAsync();
    }

    public async Task<PointOfInterestCategory?> GetByIdAsync(string id)
    {
        return await _categories.Find(c => c.Id == id).FirstOrDefaultAsync();
    }
}
