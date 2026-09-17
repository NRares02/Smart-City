using MongoDB.Driver;
using SmartCity.Api.Models;

namespace SmartCity.Api.Repositories;

public class ReportConfirmationRepository
{
    private readonly IMongoCollection<ReportConfirmation> _reportConfirmations;

    public ReportConfirmationRepository(IMongoDatabase database)
    {
        _reportConfirmations = database.GetCollection<ReportConfirmation>("report_confirmations");
    }

    public async Task<ReportConfirmation?> GetByReportAndUserAsync(string reportId, string userId)
    {
        return await _reportConfirmations
            .Find(c => c.ReportId == reportId && c.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<ReportConfirmation> CreateAsync(ReportConfirmation confirmation)
    {
        await _reportConfirmations.InsertOneAsync(confirmation);
        return confirmation;
    }

    public async Task<bool> DeleteAsync(string reportId, string userId)
    {
        var result = await _reportConfirmations.DeleteOneAsync(
            c => c.ReportId == reportId && c.UserId == userId);

        return result.DeletedCount > 0;
    }
}
