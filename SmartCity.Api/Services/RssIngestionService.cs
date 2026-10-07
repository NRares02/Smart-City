using System.Net;
using System.Security.Cryptography;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.Options;
using SmartCity.Api.Configuration;
using SmartCity.Api.Models;

namespace SmartCity.Api.Services;

public class RssFeedIngestionResult
{
    public int Fetched { get; set; }
    public int Relevant { get; set; }
    public int Inserted { get; set; }
    public int Updated { get; set; }
    public int Skipped { get; set; }
    public int Failed { get; set; }
    public int ImagesBackfilled { get; set; }

    public void Add(RssFeedIngestionResult other)
    {
        Fetched += other.Fetched;
        Relevant += other.Relevant;
        Inserted += other.Inserted;
        Updated += other.Updated;
        Skipped += other.Skipped;
        Failed += other.Failed;
        ImagesBackfilled += other.ImagesBackfilled;
    }
}

// Fetches configured RSS/Atom feeds and stores presentation-only metadata into the
// existing external_news collection via ExternalNewsService. No article bodies are stored,
// and no article pages are scraped — only data present in the feed itself is used.
public class RssIngestionService
{
    private const string MediaRssNamespace = "http://search.yahoo.com/mrss/";
    private const string ContentModuleNamespace = "http://purl.org/rss/1.0/modules/content/";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ExternalNewsService _externalNewsService;
    private readonly RssIngestionSettings _settings;
    private readonly ILogger<RssIngestionService> _logger;

    public RssIngestionService(
        IHttpClientFactory httpClientFactory,
        ExternalNewsService externalNewsService,
        IOptions<RssIngestionSettings> options,
        ILogger<RssIngestionService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _externalNewsService = externalNewsService;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<RssFeedIngestionResult> IngestAllAsync(CancellationToken cancellationToken)
    {
        var total = new RssFeedIngestionResult();

        foreach (var feed in _settings.Feeds)
        {
            cancellationToken.ThrowIfCancellationRequested();

            RssFeedIngestionResult result;
            try
            {
                result = await IngestFeedAsync(feed, cancellationToken);
            }
            catch (Exception ex)
            {
                // A single misbehaving feed must never stop the worker or the other feeds.
                _logger.LogError(ex, "Unexpected error ingesting RSS feed '{FeedName}'.", feed.Name);
                result = new RssFeedIngestionResult { Failed = 1 };
            }

            _logger.LogInformation(
                "RSS feed '{FeedName}': fetched={Fetched} relevant={Relevant} inserted={Inserted} updated={Updated} skipped={Skipped} failed={Failed} imagesBackfilled={ImagesBackfilled}",
                feed.Name, result.Fetched, result.Relevant, result.Inserted, result.Updated, result.Skipped, result.Failed, result.ImagesBackfilled);

            total.Add(result);
        }

        return total;
    }

    private async Task<RssFeedIngestionResult> IngestFeedAsync(RssFeedSettings feed, CancellationToken cancellationToken)
    {
        var result = new RssFeedIngestionResult();

        if (!feed.Enabled)
        {
            _logger.LogInformation("Skipping RSS feed '{FeedName}': disabled in appsettings.", feed.Name);
            return result;
        }

        if (string.IsNullOrWhiteSpace(feed.Url))
        {
            _logger.LogWarning("Skipping RSS feed '{FeedName}': no Url configured in appsettings.", feed.Name);
            return result;
        }

        SyndicationFeed syndicationFeed;
        try
        {
            var httpClient = _httpClientFactory.CreateClient("RssIngestion");
            await using var stream = await httpClient.GetStreamAsync(feed.Url, cancellationToken);
            using var xmlReader = XmlReader.Create(stream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, Async = true });
            syndicationFeed = SyndicationFeed.Load(xmlReader);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch/parse RSS feed '{FeedName}' from {Url}.", feed.Name, feed.Url);
            result.Failed++;
            return result;
        }

        var cutoff = DateTime.UtcNow - TimeSpan.FromDays(Math.Max(0, _settings.MaxArticleAgeDays));

        foreach (var item in syndicationFeed.Items)
        {
            result.Fetched++;
            try
            {
                var publishedAt = GetPublishedAt(item);
                var hasReliablePublishedAt = item.PublishDate != default || item.LastUpdatedTime != default;
                if (publishedAt < cutoff)
                {
                    result.Skipped++;
                    continue;
                }

                var news = BuildNewsFromItem(item, feed);
                if (news is null)
                {
                    result.Skipped++;
                    continue;
                }

                if (!IsRelevant(item, news.Title, news.Summary))
                {
                    result.Skipped++;
                    continue;
                }
                result.Relevant++;

                var existing = await _externalNewsService.FindByDedupKeysAsync(news.ExternalId, news.SourceUrl);

                // RSS feeds don't always carry an image; fall back to the article page's own
                // og:image/twitter:image metadata, but only when neither the feed nor the stored
                // document already has one (avoids a network round-trip on every refresh cycle).
                var needsImageLookup = string.IsNullOrWhiteSpace(news.ImageUrl)
                    && (existing is null || string.IsNullOrWhiteSpace(existing.ImageUrl));
                if (needsImageLookup)
                {
                    news.ImageUrl = await TryFetchLeadImageUrlAsync(news.SourceUrl, cancellationToken);
                    if (!string.IsNullOrWhiteSpace(news.ImageUrl))
                    {
                        result.ImagesBackfilled++;
                    }
                }

                if (existing is not null)
                {
                    // Never insert a second document; only refresh fields that are safe/useful to refresh.
                    var imageUrlIfMissing = string.IsNullOrWhiteSpace(existing.ImageUrl) ? news.ImageUrl : null;
                    var publishedAtIfReliable = hasReliablePublishedAt && existing.PublishedAt != news.PublishedAt
                        ? news.PublishedAt
                        : (DateTime?)null;

                    await _externalNewsService.UpdateIngestionMetadataAsync(
                        existing.Id!, news.Summary, news.Category, imageUrlIfMissing, publishedAtIfReliable);
                    result.Updated++;
                    continue;
                }

                await _externalNewsService.CreateFromIngestionAsync(news);
                result.Inserted++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process an item from RSS feed '{FeedName}'.", feed.Name);
                result.Failed++;
            }
        }

        return result;
    }

    private ExternalNews? BuildNewsFromItem(SyndicationItem item, RssFeedSettings feed)
    {
        var title = StripHtml(item.Title?.Text).Trim();
        var sourceUrl = item.Links.FirstOrDefault(l => l.RelationshipType is null or "alternate")?.Uri?.ToString()
            ?? item.Links.FirstOrDefault()?.Uri?.ToString()
            ?? string.Empty;

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(sourceUrl))
        {
            return null;
        }

        var publishedAt = GetPublishedAt(item);
        var dedupKey = !string.IsNullOrWhiteSpace(item.Id)
            ? item.Id
            : !string.IsNullOrWhiteSpace(sourceUrl)
                ? sourceUrl
                : ComputeDeterministicHash(title + publishedAt.Ticks);

        var externalId = $"{feed.Name}:{dedupKey}";

        var rawSummary = item.Summary?.Text ?? (item.Content as TextSyndicationContent)?.Text;
        var summary = BuildSummary(rawSummary, _settings.SummaryMaxLength);

        return new ExternalNews
        {
            ExternalId = externalId,
            Title = title,
            Summary = summary,
            ImageUrl = ExtractImageUrl(item),
            SourceName = feed.Name,
            SourceUrl = sourceUrl,
            Category = feed.Category,
            PublishedAt = publishedAt,
            ExpiresAt = publishedAt.AddDays(Math.Max(0, _settings.RetentionDays)),
            IsVisible = true,
            Audit = new Audit
            {
                InsertedBy = null,
                ModifiedBy = null,
                InsertedAt = DateTime.UtcNow,
                ModifiedAt = null
            }
        };
    }

    private bool IsRelevant(SyndicationItem item, string title, string summary)
    {
        var categories = string.Join(' ', item.Categories.Select(c => c.Name));
        return RssRelevanceMatcher.IsRelevant(_settings.RelevanceKeywords, title, summary, categories);
    }

    private static DateTime GetPublishedAt(SyndicationItem item)
    {
        if (item.PublishDate != default)
        {
            return item.PublishDate.UtcDateTime;
        }
        if (item.LastUpdatedTime != default)
        {
            return item.LastUpdatedTime.UtcDateTime;
        }
        return DateTime.UtcNow;
    }

    private static string? ExtractImageUrl(SyndicationItem item)
    {
        var mediaExtension = item.ElementExtensions.FirstOrDefault(e =>
            e.OuterNamespace == MediaRssNamespace && (e.OuterName == "content" || e.OuterName == "thumbnail"));
        if (mediaExtension is not null)
        {
            using var reader = mediaExtension.GetReader();
            var element = XElement.Load(reader);
            var url = element.Attribute("url")?.Value;
            if (!string.IsNullOrWhiteSpace(url))
            {
                return url;
            }
        }

        var enclosure = item.Links.FirstOrDefault(l =>
            l.RelationshipType == "enclosure" && l.MediaType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == true);
        if (enclosure is not null)
        {
            return enclosure.Uri.ToString();
        }

        // content:encoded (common WordPress/RSS extension) often carries the full HTML body
        // with the lead image, even when <description> is a short plain-text summary.
        foreach (var html in new[] { GetContentEncodedHtml(item), item.Summary?.Text, (item.Content as TextSyndicationContent)?.Text })
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                continue;
            }

            var match = Regex.Match(html, "<img[^>]+src=[\"']([^\"']+)[\"']", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
        }

        return null;
    }

    // Fallback for feeds/articles with no usable image in the RSS item itself: fetch the article
    // page and read only its <head> metadata (og:image, then twitter:image). The page body is never
    // read beyond </head> (or a small safety cap) and nothing but the resulting URL is kept.
    private async Task<string?> TryFetchLeadImageUrlAsync(string sourceUrl, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(sourceUrl) || !Uri.TryCreate(sourceUrl, UriKind.Absolute, out _))
        {
            return null;
        }

        try
        {
            var httpClient = _httpClientFactory.CreateClient("RssIngestion");
            using var response = await httpClient.GetAsync(sourceUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var contentType = response.Content.Headers.ContentType?.MediaType;
            if (contentType is not null && !contentType.Contains("html", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var head = await ReadHeadMarkupAsync(response, cancellationToken);
            if (string.IsNullOrWhiteSpace(head))
            {
                return null;
            }

            var ogImage = ExtractMetaContent(head, "og:image");
            var imageUrl = ogImage ?? ExtractMetaContent(head, "twitter:image");
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                _logger.LogInformation(
                    "Resolved fallback image for {SourceUrl} via {MetaTag}.", sourceUrl, ogImage is not null ? "og:image" : "twitter:image");
            }

            return MakeAbsoluteUrl(imageUrl, sourceUrl);
        }
        catch (Exception ex)
        {
            // Image lookup is best-effort: never let a failed fetch stop ingestion of the article.
            _logger.LogWarning(ex, "Failed to fetch lead image metadata from {SourceUrl}.", sourceUrl);
            return null;
        }
    }

    private static async Task<string> ReadHeadMarkupAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        const int maxChars = 200_000; // generous cap for real-world <head> sections; stops a runaway download

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

        var buffer = new char[4096];
        var sb = new StringBuilder();
        int read;
        while (sb.Length < maxChars && (read = await reader.ReadAsync(buffer, cancellationToken)) > 0)
        {
            sb.Append(buffer, 0, read);
            var headEnd = sb.ToString().IndexOf("</head>", StringComparison.OrdinalIgnoreCase);
            if (headEnd >= 0)
            {
                return sb.ToString(0, headEnd);
            }
        }

        return sb.ToString();
    }

    private static string? ExtractMetaContent(string headHtml, string propertyName)
    {
        var escapedName = Regex.Escape(propertyName);

        var propertyFirst = Regex.Match(
            headHtml,
            $@"<meta\s[^>]*?(?:property|name)=[""']{escapedName}[""'][^>]*?\scontent=[""']([^""']+)[""'][^>]*>",
            RegexOptions.IgnoreCase);
        if (propertyFirst.Success)
        {
            return WebUtility.HtmlDecode(propertyFirst.Groups[1].Value);
        }

        var contentFirst = Regex.Match(
            headHtml,
            $@"<meta\s[^>]*?\scontent=[""']([^""']+)[""'][^>]*?(?:property|name)=[""']{escapedName}[""'][^>]*>",
            RegexOptions.IgnoreCase);
        return contentFirst.Success ? WebUtility.HtmlDecode(contentFirst.Groups[1].Value) : null;
    }

    private static string? MakeAbsoluteUrl(string? url, string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        if (Uri.TryCreate(url, UriKind.Absolute, out var absolute))
        {
            return absolute.ToString();
        }

        if (Uri.TryCreate(baseUrl, UriKind.Absolute, out var baseUri) &&
            Uri.TryCreate(baseUri, url, out var resolved))
        {
            return resolved.ToString();
        }

        return null;
    }

    private static string? GetContentEncodedHtml(SyndicationItem item)
    {
        var encodedExtension = item.ElementExtensions.FirstOrDefault(e =>
            e.OuterNamespace == ContentModuleNamespace && e.OuterName == "encoded");
        if (encodedExtension is null)
        {
            return null;
        }

        using var reader = encodedExtension.GetReader();
        return XElement.Load(reader).Value;
    }

    private static string StripHtml(string? html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }

        var noTags = Regex.Replace(html, "<[^>]+>", " ");
        var decoded = WebUtility.HtmlDecode(noTags);
        return Regex.Replace(decoded, @"\s+", " ").Trim();
    }

    private static string BuildSummary(string? rawHtml, int maxLength)
    {
        var text = StripHtml(rawHtml);
        if (text.Length <= maxLength)
        {
            return text;
        }

        var truncated = text[..maxLength];
        var lastSpace = truncated.LastIndexOf(' ');
        if (lastSpace > 0)
        {
            truncated = truncated[..lastSpace];
        }
        return truncated.TrimEnd() + "…";
    }

    private static string ComputeDeterministicHash(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash)[..16];
    }
}
