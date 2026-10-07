using SmartCity.Api.Models;
using SmartCity.Api.Repositories;

namespace SmartCity.Api.Services;

public class ExternalNewsService
{
    private readonly ExternalNewsRepository _externalNewsRepository;

    public ExternalNewsService(ExternalNewsRepository externalNewsRepository)
    {
        _externalNewsRepository = externalNewsRepository;
    }

    public async Task<List<ExternalNews>> GetVisibleAsync()
    {
        return await _externalNewsRepository.GetVisibleAsync();
    }

    public async Task<ExternalNews?> GetByIdAsync(string id)
    {
        return await _externalNewsRepository.GetByIdAsync(id);
    }

    // Used by RSS ingestion to avoid inserting duplicates (matched by external id or source url).
    public async Task<bool> ExistsAsync(string? externalId, string? sourceUrl)
    {
        return await _externalNewsRepository.ExistsAsync(externalId, sourceUrl);
    }

    // Used by RSS ingestion to find the existing document and update it instead of inserting a duplicate.
    public async Task<ExternalNews?> FindByDedupKeysAsync(string? externalId, string? sourceUrl)
    {
        return await _externalNewsRepository.GetByDedupKeysAsync(externalId, sourceUrl);
    }

    public async Task UpdateIngestionMetadataAsync(
        string id,
        string summary,
        string category,
        string? imageUrlIfMissing,
        DateTime? publishedAtIfReliable)
    {
        await _externalNewsRepository.UpdateIngestionMetadataAsync(id, summary, category, imageUrlIfMissing, publishedAtIfReliable);
    }

    // Used only by the temporary/manual relevance-audit cleanup.
    public async Task<List<ExternalNews>> GetAllAsync()
    {
        return await _externalNewsRepository.GetAllAsync();
    }

    public async Task HideManyAsync(IEnumerable<string> ids)
    {
        await _externalNewsRepository.HideManyAsync(ids);
    }

    // Used by RSS ingestion, which builds a fully-populated ExternalNews itself (no admin user involved).
    public async Task<ExternalNews> CreateFromIngestionAsync(ExternalNews news)
    {
        return await _externalNewsRepository.CreateAsync(news);
    }

    public async Task<ExternalNews> CreateAsync(
        string adminUserId,
        string externalId,
        string title,
        string summary,
        string? imageUrl,
        string sourceName,
        string sourceUrl,
        string category,
        DateTime publishedAt,
        DateTime? expiresAt)
    {
        var existing = await _externalNewsRepository.GetByExternalIdAsync(externalId);
        if (existing is not null)
        {
            throw new InvalidOperationException("External news with this external id already exists.");
        }

        var news = new ExternalNews
        {
            ExternalId = externalId,
            Title = title,
            Summary = summary,
            ImageUrl = imageUrl,
            SourceName = sourceName,
            SourceUrl = sourceUrl,
            Category = category,
            PublishedAt = publishedAt,
            ExpiresAt = expiresAt,
            IsVisible = true,
            Audit = new Audit
            {
                InsertedBy = adminUserId,
                ModifiedBy = null,
                InsertedAt = DateTime.UtcNow,
                ModifiedAt = null
            }
        };

        return await _externalNewsRepository.CreateAsync(news);
    }
}
