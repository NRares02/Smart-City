using MongoDB.Driver;
using SmartCity.Api.Models;

namespace SmartCity.Api.Repositories;

public class NotificationRepository
{
    private readonly IMongoCollection<Notification> _notifications;

    public NotificationRepository(IMongoDatabase database)
    {
        _notifications = database.GetCollection<Notification>("notifications");
    }

    public async Task<Notification> CreateAsync(Notification notification)
    {
        await _notifications.InsertOneAsync(notification);
        return notification;
    }

    public async Task<List<Notification>> GetByUserIdAsync(string userId)
    {
        return await _notifications.Find(n => n.UserId == userId)
            .SortByDescending(n => n.Audit.InsertedAt)
            .ToListAsync();
    }

    public async Task<bool> MarkAsReadAsync(string notificationId, string userId)
    {
        var update = Builders<Notification>.Update
            .Set(n => n.ReadAt, DateTime.UtcNow)
            .Set(n => n.Audit.ModifiedBy, userId)
            .Set(n => n.Audit.ModifiedAt, DateTime.UtcNow);

        var result = await _notifications.UpdateOneAsync(
            n => n.Id == notificationId && n.UserId == userId,
            update);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> MarkAllAsReadAsync(string userId)
    {
        var update = Builders<Notification>.Update
            .Set(n => n.ReadAt, DateTime.UtcNow)
            .Set(n => n.Audit.ModifiedBy, userId)
            .Set(n => n.Audit.ModifiedAt, DateTime.UtcNow);

        var result = await _notifications.UpdateManyAsync(
            n => n.UserId == userId && n.ReadAt == null,
            update);

        return result.ModifiedCount > 0;
    }
}
