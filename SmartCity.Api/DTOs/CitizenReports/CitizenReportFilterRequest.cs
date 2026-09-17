namespace SmartCity.Api.DTOs.CitizenReports;

public class CitizenReportFilterRequest
{
    public string? ModerationStatus { get; set; }
    public string? ResolutionStatus { get; set; }
    public string? Priority { get; set; }
    public string? CategoryId { get; set; }
    public string? District { get; set; }
    public string? UserId { get; set; }
}
