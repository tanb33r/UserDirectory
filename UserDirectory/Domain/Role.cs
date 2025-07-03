﻿using MongoDB.Bson.Serialization.Attributes;

namespace UserDirectory.Domain;

[BsonIgnoreExtraElements]
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}