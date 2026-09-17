using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace SmartCity.Api.Models;

public class PointOfInterest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("category_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("location")]
    public GeoJsonPoint<GeoJson2DGeographicCoordinates>? Location { get; set; }

    [BsonElement("address")]
    public string? Address { get; set; }

    [BsonElement("image_url")]
    public string? ImageUrl { get; set; }

    [BsonElement("phone_number")]
    public string? PhoneNumber { get; set; }

    [BsonElement("website_url")]
    public string? WebsiteUrl { get; set; }

    [BsonElement("opening_hours")]
    public string? OpeningHours { get; set; }

    [BsonElement("valid_from")]
    public DateTime? ValidFrom { get; set; }

    [BsonElement("valid_until")]
    public DateTime? ValidUntil { get; set; }

    [BsonElement("is_temporary")]
    public bool IsTemporary { get; set; }

    [BsonElement("is_active")]
    public bool IsActive { get; set; }

    [BsonElement("audit")]
    public Audit Audit { get; set; } = new();
}
