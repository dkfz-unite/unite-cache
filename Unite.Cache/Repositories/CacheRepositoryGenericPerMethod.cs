using System.Linq.Expressions;
using MongoDB.Driver;
using Unite.Cache.Configuration.Options;

namespace Unite.Cache.Repositories;

public abstract class CacheRepositoryGenericPerMethod
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;

    protected abstract string DatabaseName { get; }
    protected abstract string CollectionName { get; }
    
    protected IMongoCollection<BsonEntity<T>> GetCollection<T>()
    {
        return _database.GetCollection<BsonEntity<T>>(CollectionName);
    }

    protected CacheRepositoryGenericPerMethod(IMongoOptions options)
    {
        var connectionString = $"mongodb://{options.User}:{options.Password}@{options.Host}:{options.Port}";

        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase(DatabaseName);
    }
    
    public virtual BsonEntity<TDocument> Find<TDocument>(string id)
    {
        var collection = GetCollection<TDocument>();
        var entity = collection.Find(entity => entity.Id == id).FirstOrDefault();

        return entity;
    }

    public virtual BsonEntity<TDocument> FirstOrDefault<TDocument>()
    {
        var entry = FirstOrDefault<TDocument>(entry => true);

        return entry;
    }

    public virtual BsonEntity<TDocument> FirstOrDefault<TDocument>(Expression<Func<BsonEntity<TDocument>, bool>> predicate)
    {
        var collection = GetCollection<TDocument>();
        var entry = collection.Find(predicate).FirstOrDefault();

        return entry;
    }

    public virtual IEnumerable<BsonEntity<TDocument>> Where<TDocument>(Expression<Func<BsonEntity<TDocument>, bool>> predicate)
    {
        var collection = GetCollection<TDocument>();
        var entry = collection.Find(predicate).ToList();

        return entry;
    }

    public virtual async Task<IEnumerable<BsonEntity<TDocument>>> WhereAsync<TDocument>(Expression<Func<BsonEntity<TDocument>, bool>> predicate)
    {
        var collection = GetCollection<TDocument>();
        var entry = await collection.Find(predicate).ToListAsync();

        return entry;
    }

    public virtual bool Any<TDocument>()
    {
        var collection = GetCollection<TDocument>();
        var any = collection.Find(entry => true).Any();

        return any;
    }

    public virtual bool Any<TDocument>(Expression<Func<BsonEntity<TDocument>, bool>> predicate)
    {
        var collection = GetCollection<TDocument>();
        var any = collection.Find(predicate).Any();

        return any;
    }
    
    public virtual string Add<TDocument>(TDocument document)
    {
        var collection = GetCollection<TDocument>();
        
        var entity = new BsonEntity<TDocument>(document);
        collection.InsertOne(entity);
        
        return entity.Id;
    }
    
    public virtual async Task<string> AddAsync<TDocument>(TDocument document)
    {
        var entity = new BsonEntity<TDocument>(document);

        var collection = GetCollection<TDocument>();
        await collection.InsertOneAsync(entity);

        return entity.Id;
    }

    public void Delete<TDocument>(string id)
    {
        var collection = GetCollection<TDocument>();
        collection.FindOneAndDelete(entity => entity.Id == id);
    }

    public async Task DeleteAsync<TDocument>(string id)
    {
        var collection = GetCollection<TDocument>();
        await collection.FindOneAndDeleteAsync(entity => entity.Id == id);
    }

    public void Update<TDocument>(string id, TDocument document)
    {
        var update = Builders<BsonEntity<TDocument>>.Update.Set(item => item.Document, document);
        
        var collection = GetCollection<TDocument>();
        collection.UpdateOne(item => item.Id == id, update);
    }

    public async Task UpdateAsync<TDocument>(string id, TDocument document)
    {
        var update = Builders<BsonEntity<TDocument>>.Update.Set(item => item.Document, document);
        
        var collection = GetCollection<TDocument>();
        await collection.UpdateOneAsync(item => item.Id == id, update);
    }
}