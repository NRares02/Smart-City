namespace SmartCity.Api.DTOs.CitizenReports;

public class CreateCitizenReportRequest
{
    public string CategoryId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Address { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
}
