namespace SmartCity.Api.Configuration;

public class RssIngestionSettings
{
    public int RefreshIntervalMinutes { get; set; } = 60;
    public int MaxArticleAgeDays { get; set; } = 7;
    public int SummaryMaxLength { get; set; } = 500;

    // How long a newly-ingested RSS article stays visible before the Mongo TTL index removes it.
    public int RetentionDays { get; set; } = 14;

    // Case-insensitive substring match against title+summary+categories; empty list = no filtering.
    public List<string> RelevanceKeywords { get; set; } = new();
    public List<RssFeedSettings> Feeds { get; set; } = new();
}

public class RssFeedSettings
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Category { get; set; } = "general";
    public bool Enabled { get; set; } = true;
}
