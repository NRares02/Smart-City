using SmartCity.Api.Models;
using SmartCity.Api.Repositories;

namespace SmartCity.Api.Services;

public class ReportStatusHistoryService
{
    private static readonly string[] AllowedResolutionStatuses =
    {
        "reported",
        "in_progress",
        "resolved",
        "closed"
    };

    private readonly CitizenReportRepository _citizenReportRepository;
    private readonly ReportStatusHistoryRepository _reportStatusHistoryRepository;

    public ReportStatusHistoryService(
        CitizenReportRepository citizenReportRepository,
        ReportStatusHistoryRepository reportStatusHistoryRepository)
    {
        _citizenReportRepository = citizenReportRepository;
        _reportStatusHistoryRepository = reportStatusHistoryRepository;
    }

    public async Task UpdateStatusAsync(string reportId, string userId, string newStatus, string? note)
    {
        var report = await _citizenReportRepository.GetByIdAsync(reportId);
        if (report is null)
        {
            throw new KeyNotFoundException("Report not found.");
        }

        if (!AllowedResolutionStatuses.Contains(newStatus))
        {
            throw new ArgumentException("Invalid resolution status.");
        }

        if (report.ResolutionStatus == newStatus)
        {
            throw new InvalidOperationException("Report already has this resolution status.");
        }

        var oldStatus = report.ResolutionStatus;

        var updated = await _citizenReportRepository.UpdateResolutionStatusAsync(reportId, newStatus, userId);
        if (!updated)
        {
            throw new InvalidOperationException("Failed to update report status.");
        }

        var history = new ReportStatusHistory
        {
            ReportId = reportId,
            PreviousStatus = oldStatus,
            NewStatus = newStatus,
            ChangedBy = userId,
            Comment = note,
            Audit = new Audit
            {
                InsertedBy = userId,
                ModifiedBy = null,
                InsertedAt = DateTime.UtcNow,
                ModifiedAt = null
            }
        };

        await _reportStatusHistoryRepository.CreateAsync(history);
    }

    public async Task<List<ReportStatusHistory>> GetHistoryAsync(string reportId)
    {
        var report = await _citizenReportRepository.GetByIdAsync(reportId);
        if (report is null)
        {
            throw new KeyNotFoundException("Report not found.");
        }

        return await _reportStatusHistoryRepository.GetByReportIdAsync(reportId);
    }
}
