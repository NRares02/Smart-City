using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCity.Api.Models;

public class PointOfInterestCategory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("icon")]
    public string? Icon { get; set; }

    [BsonElement("marker_color")]
    public string? MarkerColor { get; set; }

    [BsonElement("is_active")]
    public bool IsActive { get; set; }

    [BsonElement("display_order")]
    public int DisplayOrder { get; set; }

    [BsonElement("audit")]
    public Audit Audit { get; set; } = new();
}
