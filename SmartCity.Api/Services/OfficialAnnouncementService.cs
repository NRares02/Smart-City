using SmartCity.Api.Models;
using SmartCity.Api.Repositories;

namespace SmartCity.Api.Services;

public class OfficialAnnouncementService
{
    private readonly OfficialAnnouncementRepository _officialAnnouncementRepository;

    public OfficialAnnouncementService(OfficialAnnouncementRepository officialAnnouncementRepository)
    {
        _officialAnnouncementRepository = officialAnnouncementRepository;
    }

    public async Task<List<OfficialAnnouncement>> GetPublishedAsync()
    {
        return await _officialAnnouncementRepository.GetPublishedAsync();
    }

    public async Task<OfficialAnnouncement> CreateAsync(
        string adminUserId,
        string title,
        string summary,
        string content,
        string category,
        string? imageUrl,
        bool isPinned,
        DateTime? expiresAt)
    {
        var announcement = new OfficialAnnouncement
        {
            Title = title,
            Summary = summary,
            Content = content,
            Category = category,
            ImageUrl = imageUrl,
            AuthorId = adminUserId,
            IsPublished = true,
            IsPinned = isPinned,
            ExpiresAt = expiresAt,
            Audit = new Audit
            {
                InsertedBy = adminUserId,
                ModifiedBy = null,
                InsertedAt = DateTime.UtcNow,
                ModifiedAt = null
            }
        };

        return await _officialAnnouncementRepository.CreateAsync(announcement);
    }

    public async Task SetPublishedAsync(
        string announcementId,
        string adminUserId,
        bool isPublished)
    {
        var announcement = await _officialAnnouncementRepository.GetByIdAsync(announcementId);
        if (announcement is null)
        {
            throw new KeyNotFoundException("Announcement not found.");
        }

        var updated = await _officialAnnouncementRepository.SetPublishedAsync(announcementId, isPublished, adminUserId);
        if (!updated)
        {
            throw new InvalidOperationException("Failed to update announcement publication status.");
        }
    }
}
