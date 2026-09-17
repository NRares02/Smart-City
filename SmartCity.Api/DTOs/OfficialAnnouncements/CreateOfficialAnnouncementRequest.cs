namespace SmartCity.Api.DTOs.OfficialAnnouncements;

public class CreateOfficialAnnouncementRequest
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsPinned { get; set; }
    public DateTime? ExpiresAt { get; set; }
}
