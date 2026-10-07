using MongoDB.Driver;
using SmartCity.Api.Models;

namespace SmartCity.Api.Repositories;

public class ExternalNewsRepository
{
    private readonly IMongoCollection<ExternalNews> _externalNews;

    public ExternalNewsRepository(IMongoDatabase database)
    {
        _externalNews = database.GetCollection<ExternalNews>("external_news");
    }

    public async Task<List<ExternalNews>> GetVisibleAsync()
    {
        var now = DateTime.UtcNow;

        var filter = Builders<ExternalNews>.Filter.And(
            Builders<ExternalNews>.Filter.Eq(n => n.IsVisible, true),
            Builders<ExternalNews>.Filter.Or(
                Builders<ExternalNews>.Filter.Eq(n => n.ExpiresAt, null),
                Builders<ExternalNews>.Filter.Gt(n => n.ExpiresAt, now)));

        return await _externalNews.Find(filter)
            .SortByDescending(n => n.PublishedAt)
            .ToListAsync();
    }

    public async Task<ExternalNews?> GetByIdAsync(string id)
    {
        return await _externalNews.Find(n => n.Id == id).FirstOrDefaultAsync();
    }

    public async Task<ExternalNews?> GetByExternalIdAsync(string externalId)
    {
        return await _externalNews.Find(n => n.ExternalId == externalId).FirstOrDefaultAsync();
    }

    // Used by RSS ingestion to skip articles already stored (by external id and/or source url).
    public async Task<bool> ExistsAsync(string? externalId, string? sourceUrl)
    {
        var filters = new List<FilterDefinition<ExternalNews>>();
        if (!string.IsNullOrWhiteSpace(externalId))
        {
            filters.Add(Builders<ExternalNews>.Filter.Eq(n => n.ExternalId, externalId));
        }
        if (!string.IsNullOrWhiteSpace(sourceUrl))
        {
            filters.Add(Builders<ExternalNews>.Filter.Eq(n => n.SourceUrl, sourceUrl));
        }

        if (filters.Count == 0)
        {
            return false;
        }

        var filter = Builders<ExternalNews>.Filter.Or(filters);
        return await _externalNews.Find(filter).AnyAsync();
    }

    // Used by RSS ingestion to find the existing document to update instead of inserting a duplicate.
    public async Task<ExternalNews?> GetByDedupKeysAsync(string? externalId, string? sourceUrl)
    {
        var filters = new List<FilterDefinition<ExternalNews>>();
        if (!string.IsNullOrWhiteSpace(externalId))
        {
            filters.Add(Builders<ExternalNews>.Filter.Eq(n => n.ExternalId, externalId));
        }
        if (!string.IsNullOrWhiteSpace(sourceUrl))
        {
            filters.Add(Builders<ExternalNews>.Filter.Eq(n => n.SourceUrl, sourceUrl));
        }

        if (filters.Count == 0)
        {
            return null;
        }

        var filter = Builders<ExternalNews>.Filter.Or(filters);
        return await _externalNews.Find(filter).FirstOrDefaultAsync();
    }

    // Refreshes presentation metadata on a re-ingested article without touching external_id/source_url.
    // imageUrlIfMissing/publishedAtIfReliable are passed as null by the caller when that field should be left alone.
    public async Task UpdateIngestionMetadataAsync(
        string id,
        string summary,
        string category,
        string? imageUrlIfMissing,
        DateTime? publishedAtIfReliable)
    {
        var updates = new List<UpdateDefinition<ExternalNews>>
        {
            Builders<ExternalNews>.Update.Set(n => n.Summary, summary),
            Builders<ExternalNews>.Update.Set(n => n.Category, category),
            Builders<ExternalNews>.Update.Set(n => n.Audit.ModifiedAt, DateTime.UtcNow)
        };

        if (!string.IsNullOrWhiteSpace(imageUrlIfMissing))
        {
            updates.Add(Builders<ExternalNews>.Update.Set(n => n.ImageUrl, imageUrlIfMissing));
        }

        if (publishedAtIfReliable.HasValue)
        {
            updates.Add(Builders<ExternalNews>.Update.Set(n => n.PublishedAt, publishedAtIfReliable.Value));
        }

        await _externalNews.UpdateOneAsync(n => n.Id == id, Builders<ExternalNews>.Update.Combine(updates));
    }

    // Used only by the temporary/manual relevance-audit cleanup (not the scheduled ingestion worker).
    public async Task<List<ExternalNews>> GetAllAsync()
    {
        return await _externalNews.Find(FilterDefinition<ExternalNews>.Empty).ToListAsync();
    }

    // Soft-hides (is_visible=false) a batch of documents; never deletes.
    public async Task HideManyAsync(IEnumerable<string> ids)
    {
        var idList = ids.ToList();
        if (idList.Count == 0)
        {
            return;
        }

        var filter = Builders<ExternalNews>.Filter.In(n => n.Id, idList);
        var update = Builders<ExternalNews>.Update
            .Set(n => n.IsVisible, false)
            .Set(n => n.Audit.ModifiedAt, DateTime.UtcNow);

        await _externalNews.UpdateManyAsync(filter, update);
    }

    public async Task<ExternalNews> CreateAsync(ExternalNews news)
    {
        await _externalNews.InsertOneAsync(news);
        return news;
    }

    // Idempotent: safe to call on every startup. Docs with expires_at=null are never touched by
    // the TTL monitor since Mongo only expires indexed fields holding an actual Date value.
    public async Task EnsureIndexesAsync()
    {
        const string ttlIndexName = "expires_at_ttl";

        // Replace any pre-existing plain (non-TTL) index on expires_at: Mongo refuses to create
        // an equivalent index under a new name/options, so a same-key index must be dropped first.
        using (var cursor = await _externalNews.Indexes.ListAsync())
        {
            var existingIndexes = await cursor.ToListAsync();
            foreach (var index in existingIndexes)
            {
                var name = index["name"].AsString;
                if (name == ttlIndexName)
                {
                    return;
                }

                var key = index["key"].AsBsonDocument;
                if (key.ElementCount == 1 && key.Contains("expires_at"))
                {
                    await _externalNews.Indexes.DropOneAsync(name);
                }
            }
        }

        var ttlIndex = new CreateIndexModel<ExternalNews>(
            Builders<ExternalNews>.IndexKeys.Ascending(n => n.ExpiresAt),
            new CreateIndexOptions { Name = ttlIndexName, ExpireAfter = TimeSpan.Zero });

        await _externalNews.Indexes.CreateOneAsync(ttlIndex);
    }
}
