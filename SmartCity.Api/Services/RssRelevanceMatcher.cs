namespace SmartCity.Api.Services;

// Shared by RssIngestionService (live feed items) and ExternalNewsRelevanceAuditService
// (already-stored documents) so both use the exact same matching rule.
public static class RssRelevanceMatcher
{
    public static bool IsRelevant(IReadOnlyCollection<string> keywords, string title, string summary, string categories)
    {
        if (keywords is null || keywords.Count == 0)
        {
            return true;
        }

        var haystack = string.Join(' ', title, summary, categories);
        return keywords.Any(keyword =>
            !string.IsNullOrWhiteSpace(keyword) && haystack.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }
}
