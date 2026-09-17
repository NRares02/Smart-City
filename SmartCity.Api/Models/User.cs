using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCity.Api.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("email")]
    public string? Email { get; set; }

    [BsonElement("password_hash")]
    public string? PasswordHash { get; set; }

    [BsonElement("phone_number")]
    public string? PhoneNumber { get; set; }

    [BsonElement("role_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? RoleId { get; set; }

    [BsonElement("profile_image_url")]
    public string? ProfileImageUrl { get; set; }

    [BsonElement("is_active")]
    public bool IsActive { get; set; }

    [BsonElement("audit")]
    public Audit Audit { get; set; } = new();
}
