using MongoDB.Driver;
using SmartCity.Api.Models;

namespace SmartCity.Api.Repositories;

public class ReportStatusHistoryRepository
{
    private readonly IMongoCollection<ReportStatusHistory> _reportStatusHistory;

    public ReportStatusHistoryRepository(IMongoDatabase database)
    {
        _reportStatusHistory = database.GetCollection<ReportStatusHistory>("report_status_history");
    }

    public async Task<ReportStatusHistory> CreateAsync(ReportStatusHistory history)
    {
        await _reportStatusHistory.InsertOneAsync(history);
        return history;
    }

    public async Task<List<ReportStatusHistory>> GetByReportIdAsync(string reportId)
    {
        return await _reportStatusHistory.Find(h => h.ReportId == reportId)
            .SortBy(h => h.Audit.InsertedAt)
            .ToListAsync();
    }
}
