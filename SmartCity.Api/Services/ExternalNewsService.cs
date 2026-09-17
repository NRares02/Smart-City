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
