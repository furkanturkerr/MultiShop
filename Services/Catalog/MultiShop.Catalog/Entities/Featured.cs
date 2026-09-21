using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiShop.Catalog.Entities;

public class Featured
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string FeaturedId { get; set; }
    public string Icon { get; set; }
    public string Title { get; set; }
    
    public bool IsActive { get; set; }
}