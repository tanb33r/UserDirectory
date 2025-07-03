namespace UserDirectory.Domain;

using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Contact
{
    [BsonElement("Id")]
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}

