using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Unite.Cache.Repositories;

public class BsonEntity<T>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public DateTime Date { get; set; }

    public T Document { get; set; }


    public BsonEntity()
    {
        Date = DateTime.UtcNow;
    }

    public BsonEntity(T document) : this()
    {
        Document = document;
    }
}
