using MongoDB.Driver;
using Unite.Cache.Configuration.Options;

namespace Unite.Cache.Repositories;

public abstract class CacheRepository2
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;

    protected abstract string DatabaseName { get; }
    protected abstract string CollectionName { get; }

    protected CacheRepository2(IMongoOptions options)
    {
        var connectionString = $"mongodb://{options.User}:{options.Password}@{options.Host}:{options.Port}";

        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase(DatabaseName);
    }
    
    public virtual string Add<TDocument>(TDocument document)
    {
        var collection = _database.GetCollection<BsonEntity<TDocument>>(CollectionName);
        
        var entity = new BsonEntity<TDocument>(document);
        collection.InsertOne(entity);
        
        return entity.Id;
    }
}