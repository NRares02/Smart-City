using MongoDB.Driver;
using SmartCity.Api.Models;

namespace SmartCity.Api.Repositories;

public class ExternalNewsRepository
{
    private readonly IMongoCollection<ExternalNews> _externalNews;

    public ExternalNewsRepository(IMongoDatabase database)
    {
        _externalNews = database.GetCollection<ExternalNews>("external_news");
    }

    public async Task<List<ExternalNews>> GetVisibleAsync()
    {
        var now = DateTime.UtcNow;

        var filter = Builders<ExternalNews>.Filter.And(
            Builders<ExternalNews>.Filter.Eq(n => n.IsVisible, true),
            Builders<ExternalNews>.Filter.Or(
                Builders<ExternalNews>.Filter.Eq(n => n.ExpiresAt, null),
                Builders<ExternalNews>.Filter.Gt(n => n.ExpiresAt, now)));

        return await _externalNews.Find(filter)
            .SortByDescending(n => n.PublishedAt)
            .ToListAsync();
    }

    public async Task<ExternalNews?> GetByIdAsync(string id)
    {
        return await _externalNews.Find(n => n.Id == id).FirstOrDefaultAsync();
    }

    public async Task<ExternalNews?> GetByExternalIdAsync(string externalId)
    {
        return await _externalNews.Find(n => n.ExternalId == externalId).FirstOrDefaultAsync();
    }

    public async Task<ExternalNews> CreateAsync(ExternalNews news)
    {
        await _externalNews.InsertOneAsync(news);
        return news;
    }
}
