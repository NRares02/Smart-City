using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace SmartCity.Api.Models;

public class CitizenReport
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = string.Empty;

    [BsonElement("category_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; } = string.Empty;

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("location")]
    public GeoJsonPoint<GeoJson2DGeographicCoordinates>? Location { get; set; }

    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;

    [BsonElement("district")]
    public string District { get; set; } = string.Empty;

    [BsonElement("moderation_status")]
    public string ModerationStatus { get; set; } = string.Empty;

    [BsonElement("resolution_status")]
    public string ResolutionStatus { get; set; } = string.Empty;

    [BsonElement("priority")]
    public string Priority { get; set; } = string.Empty;

    [BsonElement("is_public")]
    public bool IsPublic { get; set; }

    [BsonElement("approved_by")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ApprovedBy { get; set; }

    [BsonElement("approved_at")]
    public DateTime? ApprovedAt { get; set; }

    [BsonElement("resolved_at")]
    public DateTime? ResolvedAt { get; set; }

    [BsonElement("assigned_to")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? AssignedTo { get; set; }

    [BsonElement("resolution_summary")]
    public string? ResolutionSummary { get; set; }

    [BsonElement("confirmation_count")]
    public int ConfirmationCount { get; set; }

    [BsonElement("audit")]
    public Audit Audit { get; set; } = new();
}
