using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiShop.Catalog.Entities;

public class OfferDiscount
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string OfferDiscountId { get; set; }
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string ButtonText { get; set; }
}