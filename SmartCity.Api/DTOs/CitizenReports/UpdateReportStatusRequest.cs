namespace SmartCity.Api.DTOs.CitizenReports;

public class UpdateReportStatusRequest
{
    public string ResolutionStatus { get; set; } = string.Empty;
    public string? Note { get; set; }
}
