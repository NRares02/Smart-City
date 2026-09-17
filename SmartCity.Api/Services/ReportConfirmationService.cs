using SmartCity.Api.Models;
using SmartCity.Api.Repositories;

namespace SmartCity.Api.Services;

public class ReportConfirmationService
{
    private readonly ReportConfirmationRepository _reportConfirmationRepository;
    private readonly CitizenReportRepository _citizenReportRepository;

    public ReportConfirmationService(
        ReportConfirmationRepository reportConfirmationRepository,
        CitizenReportRepository citizenReportRepository)
    {
        _reportConfirmationRepository = reportConfirmationRepository;
        _citizenReportRepository = citizenReportRepository;
    }

    public async Task ConfirmAsync(string reportId, string userId)
    {
        var report = await _citizenReportRepository.GetByIdAsync(reportId);
        if (report is null)
        {
            throw new KeyNotFoundException("Report not found.");
        }

        var existingConfirmation = await _reportConfirmationRepository.GetByReportAndUserAsync(reportId, userId);
        if (existingConfirmation is not null)
        {
            throw new InvalidOperationException("Report already confirmed by this user.");
        }

        var confirmation = new ReportConfirmation
        {
            ReportId = reportId,
            UserId = userId,
            Audit = new Audit
            {
                InsertedBy = userId,
                ModifiedBy = null,
                InsertedAt = DateTime.UtcNow,
                ModifiedAt = null
            }
        };

        await _reportConfirmationRepository.CreateAsync(confirmation);
        await _citizenReportRepository.IncrementConfirmationCountAsync(reportId);
    }

    public async Task UnconfirmAsync(string reportId, string userId)
    {
        var report = await _citizenReportRepository.GetByIdAsync(reportId);
        if (report is null)
        {
            throw new KeyNotFoundException("Report not found.");
        }

        var deleted = await _reportConfirmationRepository.DeleteAsync(reportId, userId);
        if (!deleted)
        {
            throw new InvalidOperationException("Report confirmation does not exist.");
        }

        await _citizenReportRepository.DecrementConfirmationCountAsync(reportId);
    }
}
