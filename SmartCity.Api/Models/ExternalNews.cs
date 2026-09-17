using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCity.Api.Models;

public class ExternalNews
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("external_id")]
    public string ExternalId { get; set; } = string.Empty;

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("summary")]
    public string Summary { get; set; } = string.Empty;

    [BsonElement("image_url")]
    public string? ImageUrl { get; set; }

    [BsonElement("source_name")]
    public string SourceName { get; set; } = string.Empty;

    [BsonElement("source_url")]
    public string SourceUrl { get; set; } = string.Empty;

    [BsonElement("category")]
    public string Category { get; set; } = string.Empty;

    [BsonElement("published_at")]
    public DateTime PublishedAt { get; set; }

    [BsonElement("expires_at")]
    public DateTime? ExpiresAt { get; set; }

    [BsonElement("is_visible")]
    public bool IsVisible { get; set; }

    [BsonElement("audit")]
    public Audit Audit { get; set; } = new();
}
