using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCity.Api.Models;

public class ReportStatusHistory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("report_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ReportId { get; set; } = string.Empty;

    [BsonElement("previous_status")]
    public string? PreviousStatus { get; set; }

    [BsonElement("new_status")]
    public string NewStatus { get; set; } = string.Empty;

    [BsonElement("changed_by")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ChangedBy { get; set; } = string.Empty;

    [BsonElement("comment")]
    public string? Comment { get; set; }

    [BsonElement("audit")]
    public Audit Audit { get; set; } = new();
}
