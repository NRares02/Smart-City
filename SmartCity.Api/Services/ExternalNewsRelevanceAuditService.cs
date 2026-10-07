using Microsoft.Extensions.Options;
using SmartCity.Api.Configuration;
using SmartCity.Api.Models;

namespace SmartCity.Api.Services;

public class ExternalNewsRelevanceAuditResult
{
    public int TotalCount { get; set; }
    public int RelevantCount { get; set; }
    public int IrrelevantCount { get; set; }
    public List<ExternalNews> IrrelevantItems { get; set; } = new();
}

// TEMPORARY manual cleanup helper for the backlog of articles ingested before relevance
// filtering existed. Not wired into the scheduled RssIngestionBackgroundService — only
// triggered on demand by an admin via ExternalNewsController. Hides (never deletes) matches.
public class ExternalNewsRelevanceAuditService
{
    private readonly ExternalNewsService _externalNewsService;
    private readonly RssIngestionSettings _settings;

    public ExternalNewsRelevanceAuditService(ExternalNewsService externalNewsService, IOptions<RssIngestionSettings> options)
    {
        _externalNewsService = externalNewsService;
        _settings = options.Value;
    }

    public async Task<ExternalNewsRelevanceAuditResult> GetAuditAsync()
    {
        var all = await _externalNewsService.GetAllAsync();
        var irrelevant = all
            .Where(n => !RssRelevanceMatcher.IsRelevant(_settings.RelevanceKeywords, n.Title, n.Summary, n.Category))
            .ToList();

        return new ExternalNewsRelevanceAuditResult
        {
            TotalCount = all.Count,
            RelevantCount = all.Count - irrelevant.Count,
            IrrelevantCount = irrelevant.Count,
            IrrelevantItems = irrelevant
        };
    }

    public async Task<int> HideIrrelevantAsync()
    {
        var audit = await GetAuditAsync();
        var idsToHide = audit.IrrelevantItems.Where(n => n.IsVisible).Select(n => n.Id!).ToList();
        if (idsToHide.Count == 0)
        {
            return 0;
        }

        await _externalNewsService.HideManyAsync(idsToHide);
        return idsToHide.Count;
    }
}
