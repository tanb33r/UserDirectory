namespace UserDirectory.Domain;

using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;


[BsonIgnoreExtraElements]
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public bool Active { get; set; }
    public string Company { get; set; } = null!;
    public Sex Sex { get; set; }
    public Contact Contact { get; set; } = null!;
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}


public enum Sex
{
    M,
    F
}
