using MongoDB.Driver;
using SmartCity.Api.Models;

namespace SmartCity.Api.Repositories;

public class OfficialAnnouncementRepository
{
    private readonly IMongoCollection<OfficialAnnouncement> _officialAnnouncements;

    public OfficialAnnouncementRepository(IMongoDatabase database)
    {
        _officialAnnouncements = database.GetCollection<OfficialAnnouncement>("official_announcements");
    }

    public async Task<List<OfficialAnnouncement>> GetPublishedAsync()
    {
        var now = DateTime.UtcNow;

        var filter = Builders<OfficialAnnouncement>.Filter.And(
            Builders<OfficialAnnouncement>.Filter.Eq(a => a.IsPublished, true),
            Builders<OfficialAnnouncement>.Filter.Or(
                Builders<OfficialAnnouncement>.Filter.Eq(a => a.ExpiresAt, null),
                Builders<OfficialAnnouncement>.Filter.Gt(a => a.ExpiresAt, now)));

        return await _officialAnnouncements.Find(filter)
            .SortByDescending(a => a.IsPinned)
            .ThenByDescending(a => a.Audit.InsertedAt)
            .ToListAsync();
    }

    public async Task<OfficialAnnouncement?> GetByIdAsync(string id)
    {
        return await _officialAnnouncements.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task<OfficialAnnouncement> CreateAsync(OfficialAnnouncement announcement)
    {
        await _officialAnnouncements.InsertOneAsync(announcement);
        return announcement;
    }

    public async Task<bool> SetPublishedAsync(string id, bool isPublished, string modifiedBy)
    {
        var update = Builders<OfficialAnnouncement>.Update
            .Set(a => a.IsPublished, isPublished)
            .Set(a => a.Audit.ModifiedBy, modifiedBy)
            .Set(a => a.Audit.ModifiedAt, DateTime.UtcNow);

        var result = await _officialAnnouncements.UpdateOneAsync(a => a.Id == id, update);
        return result.ModifiedCount > 0;
    }
}
