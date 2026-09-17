using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCity.Api.Models;

public class ReportConfirmation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("report_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ReportId { get; set; } = string.Empty;

    [BsonElement("user_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = string.Empty;

    [BsonElement("audit")]
    public Audit Audit { get; set; } = new();
}
