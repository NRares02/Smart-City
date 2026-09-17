using MongoDB.Driver;
using SmartCity.Api.Models;

namespace SmartCity.Api.Repositories;

public class CitizenReportRepository
{
    private readonly IMongoCollection<CitizenReport> _citizenReports;

    public CitizenReportRepository(IMongoDatabase database)
    {
        _citizenReports = database.GetCollection<CitizenReport>("citizen_reports");
    }

    public async Task<CitizenReport?> GetByIdAsync(string id)
    {
        return await _citizenReports.Find(r => r.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<CitizenReport>> GetAllAsync()
    {
        return await _citizenReports.Find(_ => true)
            .SortByDescending(r => r.Audit.InsertedAt)
            .ToListAsync();
    }

    public async Task<List<CitizenReport>> GetByUserIdAsync(string userId)
    {
        return await _citizenReports.Find(r => r.UserId == userId)
            .SortByDescending(r => r.Audit.InsertedAt)
            .ToListAsync();
    }

    public async Task<List<CitizenReport>> GetFilteredAsync(
        string? moderationStatus,
        string? resolutionStatus,
        string? priority,
        string? categoryId,
        string? district,
        string? userId)
    {
        var filters = new List<FilterDefinition<CitizenReport>>();

        if (!string.IsNullOrWhiteSpace(moderationStatus))
        {
            filters.Add(Builders<CitizenReport>.Filter.Eq(r => r.ModerationStatus, moderationStatus));
        }

        if (!string.IsNullOrWhiteSpace(resolutionStatus))
        {
            filters.Add(Builders<CitizenReport>.Filter.Eq(r => r.ResolutionStatus, resolutionStatus));
        }

        if (!string.IsNullOrWhiteSpace(priority))
        {
            filters.Add(Builders<CitizenReport>.Filter.Eq(r => r.Priority, priority));
        }

        if (!string.IsNullOrWhiteSpace(categoryId))
        {
            filters.Add(Builders<CitizenReport>.Filter.Eq(r => r.CategoryId, categoryId));
        }

        if (!string.IsNullOrWhiteSpace(district))
        {
            filters.Add(Builders<CitizenReport>.Filter.Eq(r => r.District, district));
        }

        if (!string.IsNullOrWhiteSpace(userId))
        {
            filters.Add(Builders<CitizenReport>.Filter.Eq(r => r.UserId, userId));
        }

        var filter = filters.Count > 0
            ? Builders<CitizenReport>.Filter.And(filters)
            : Builders<CitizenReport>.Filter.Empty;

        return await _citizenReports.Find(filter)
            .SortByDescending(r => r.Audit.InsertedAt)
            .ToListAsync();
    }

    public async Task<CitizenReport> CreateAsync(CitizenReport report)
    {
        await _citizenReports.InsertOneAsync(report);
        return report;
    }

    public async Task UpdateAsync(CitizenReport report)
    {
        await _citizenReports.ReplaceOneAsync(r => r.Id == report.Id, report);
    }

    public async Task<bool> IncrementConfirmationCountAsync(string reportId)
    {
        var update = Builders<CitizenReport>.Update
            .Inc(r => r.ConfirmationCount, 1)
            .Set(r => r.Audit.ModifiedAt, DateTime.UtcNow);

        var result = await _citizenReports.UpdateOneAsync(r => r.Id == reportId, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DecrementConfirmationCountAsync(string reportId)
    {
        var update = Builders<CitizenReport>.Update
            .Inc(r => r.ConfirmationCount, -1)
            .Set(r => r.Audit.ModifiedAt, DateTime.UtcNow);

        var result = await _citizenReports.UpdateOneAsync(
            r => r.Id == reportId && r.ConfirmationCount > 0,
            update);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateResolutionStatusAsync(string reportId, string newStatus, string modifiedBy)
    {
        var update = Builders<CitizenReport>.Update
            .Set(r => r.ResolutionStatus, newStatus)
            .Set(r => r.Audit.ModifiedBy, modifiedBy)
            .Set(r => r.Audit.ModifiedAt, DateTime.UtcNow);

        var result = await _citizenReports.UpdateOneAsync(r => r.Id == reportId, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateModerationStatusAsync(string reportId, string moderationStatus, string approvedBy)
    {
        var update = Builders<CitizenReport>.Update
            .Set(r => r.ModerationStatus, moderationStatus)
            .Set(r => r.ApprovedBy, approvedBy)
            .Set(r => r.ApprovedAt, DateTime.UtcNow)
            .Set(r => r.Audit.ModifiedBy, approvedBy)
            .Set(r => r.Audit.ModifiedAt, DateTime.UtcNow);

        var result = await _citizenReports.UpdateOneAsync(r => r.Id == reportId, update);
        return result.ModifiedCount > 0;
    }
}
