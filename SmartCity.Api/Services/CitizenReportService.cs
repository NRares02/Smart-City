using MongoDB.Driver.GeoJsonObjectModel;
using SmartCity.Api.Models;
using SmartCity.Api.Repositories;

namespace SmartCity.Api.Services;

public class CitizenReportService
{
    private static readonly string[] AllowedModerationStatuses = { "approved", "rejected" };
    private static readonly string[] AllowedFilterModerationStatuses = { "pending", "approved", "rejected" };
    private static readonly string[] AllowedFilterResolutionStatuses = { "reported", "in_progress", "resolved", "closed" };
    private static readonly string[] AllowedFilterPriorities = { "low", "medium", "high", "critical" };

    private readonly CitizenReportRepository _citizenReportRepository;
    private readonly NotificationService _notificationService;

    public CitizenReportService(
        CitizenReportRepository citizenReportRepository,
        NotificationService notificationService)
    {
        _citizenReportRepository = citizenReportRepository;
        _notificationService = notificationService;
    }

    public async Task<CitizenReport?> GetByIdAsync(string id)
    {
        return await _citizenReportRepository.GetByIdAsync(id);
    }

    public async Task<List<CitizenReport>> GetAllAsync()
    {
        return await _citizenReportRepository.GetAllAsync();
    }

    public async Task<List<CitizenReport>> GetByUserIdAsync(string userId)
    {
        return await _citizenReportRepository.GetByUserIdAsync(userId);
    }

    public async Task<List<CitizenReport>> GetFilteredAsync(
        string? moderationStatus,
        string? resolutionStatus,
        string? priority,
        string? categoryId,
        string? district,
        string? userId)
    {
        if (!string.IsNullOrWhiteSpace(moderationStatus) && !AllowedFilterModerationStatuses.Contains(moderationStatus))
        {
            throw new ArgumentException("Invalid moderation status.");
        }

        if (!string.IsNullOrWhiteSpace(resolutionStatus) && !AllowedFilterResolutionStatuses.Contains(resolutionStatus))
        {
            throw new ArgumentException("Invalid resolution status.");
        }

        if (!string.IsNullOrWhiteSpace(priority) && !AllowedFilterPriorities.Contains(priority))
        {
            throw new ArgumentException("Invalid priority.");
        }

        return await _citizenReportRepository.GetFilteredAsync(
            moderationStatus,
            resolutionStatus,
            priority,
            categoryId,
            district,
            userId);
    }

    public async Task<CitizenReport> CreateAsync(
        string userId,
        string categoryId,
        string title,
        string description,
        double longitude,
        double latitude,
        string address,
        string district)
    {
        var now = DateTime.UtcNow;

        var report = new CitizenReport
        {
            UserId = userId,
            CategoryId = categoryId,
            Title = title,
            Description = description,
            Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                new GeoJson2DGeographicCoordinates(longitude, latitude)),
            Address = address,
            District = district,
            ModerationStatus = "pending",
            ResolutionStatus = "reported",
            Priority = "medium",
            IsPublic = false,
            ApprovedBy = null,
            ApprovedAt = null,
            AssignedTo = null,
            ResolvedAt = null,
            ResolutionSummary = null,
            ConfirmationCount = 0,
            Audit = new Audit
            {
                InsertedBy = userId,
                ModifiedBy = null,
                InsertedAt = now,
                ModifiedAt = now
            }
        };

        return await _citizenReportRepository.CreateAsync(report);
    }

    public async Task ModerateAsync(string reportId, string adminUserId, string moderationStatus)
    {
        var report = await _citizenReportRepository.GetByIdAsync(reportId);
        if (report is null)
        {
            throw new KeyNotFoundException("Report not found.");
        }

        if (!AllowedModerationStatuses.Contains(moderationStatus))
        {
            throw new ArgumentException("Invalid moderation status.");
        }

        if (report.ModerationStatus != "pending")
        {
            throw new InvalidOperationException("Report has already been moderated.");
        }

        var updated = await _citizenReportRepository.UpdateModerationStatusAsync(reportId, moderationStatus, adminUserId);
        if (!updated)
        {
            throw new InvalidOperationException("Failed to update report moderation status.");
        }

        await _notificationService.CreateReportModerationNotificationAsync(report.UserId, report.Id!, moderationStatus);
    }
}
