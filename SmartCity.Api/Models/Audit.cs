using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCity.Api.Models;

public class Audit
{
    [BsonElement("inserted_by")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? InsertedBy { get; set; }

    [BsonElement("modified_by")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ModifiedBy { get; set; }

    [BsonElement("inserted_at")]
    public DateTime InsertedAt { get; set; }

    [BsonElement("modified_at")]
    public DateTime? ModifiedAt { get; set; }
}
