using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCity.Api.Models;

public class ReportMessage
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("report_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ReportId { get; set; } = string.Empty;

    [BsonElement("sender_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string SenderId { get; set; } = string.Empty;

    [BsonElement("message")]
    public string Message { get; set; } = string.Empty;

    [BsonElement("is_internal")]
    public bool IsInternal { get; set; }

    [BsonElement("audit")]
    public Audit Audit { get; set; } = new();
}
