using SmartCity.Api.Models;
using SmartCity.Api.Repositories;

namespace SmartCity.Api.Services;

public class NotificationService
{
    private readonly NotificationRepository _notificationRepository;

    public NotificationService(NotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Notification> CreateReportModerationNotificationAsync(
        string userId,
        string reportId,
        string moderationStatus)
    {
        var title = moderationStatus == "approved" ? "Report approved" : "Report rejected";
        var message = moderationStatus == "approved"
            ? "Your citizen report has been approved."
            : "Your citizen report has been rejected.";

        var notification = new Notification
        {
            UserId = userId,
            ReportId = reportId,
            Title = title,
            Message = message,
            Type = "report_moderation",
            ReadAt = null,
            Audit = new Audit
            {
                InsertedBy = null,
                ModifiedBy = null,
                InsertedAt = DateTime.UtcNow,
                ModifiedAt = null
            }
        };

        return await _notificationRepository.CreateAsync(notification);
    }

    public async Task<List<Notification>> GetByUserIdAsync(string userId)
    {
        return await _notificationRepository.GetByUserIdAsync(userId);
    }

    public async Task MarkAsReadAsync(string notificationId, string userId)
    {
        var updated = await _notificationRepository.MarkAsReadAsync(notificationId, userId);
        if (!updated)
        {
            throw new KeyNotFoundException("Notification not found.");
        }
    }

    public async Task MarkAllAsReadAsync(string userId)
    {
        await _notificationRepository.MarkAllAsReadAsync(userId);
    }
}
