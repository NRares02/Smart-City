using MongoDB.Driver;
using SmartCity.Api.Models;

namespace SmartCity.Api.Repositories;

public class PointOfInterestRepository
{
    private readonly IMongoCollection<PointOfInterest> _pointsOfInterest;

    public PointOfInterestRepository(IMongoDatabase database)
    {
        _pointsOfInterest = database.GetCollection<PointOfInterest>("points_of_interest");
    }

    public async Task<List<PointOfInterest>> GetActiveAsync()
    {
        return await _pointsOfInterest.Find(p => p.IsActive == true)
            .SortByDescending(p => p.Audit.InsertedAt)
            .ToListAsync();
    }

    public async Task<List<PointOfInterest>> GetByCategoryIdAsync(string categoryId)
    {
        return await _pointsOfInterest.Find(p => p.IsActive == true && p.CategoryId == categoryId)
            .SortByDescending(p => p.Audit.InsertedAt)
            .ToListAsync();
    }

    public async Task<PointOfInterest?> GetByIdAsync(string id)
    {
        return await _pointsOfInterest.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<PointOfInterest> CreateAsync(PointOfInterest pointOfInterest)
    {
        await _pointsOfInterest.InsertOneAsync(pointOfInterest);
        return pointOfInterest;
    }
}
