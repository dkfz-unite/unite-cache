using System.Linq.Expressions;
using MongoDB.Driver;
using Unite.Cache.Configuration.Options;

namespace Unite.Cache.Repositories;

public abstract class CacheRepository<T> where T : class
{
    protected readonly IMongoClient _client;
    protected readonly IMongoDatabase _database;
    protected readonly IMongoCollection<BsonEntity<T>> _collection;

    public abstract string DatabaseName { get; }
    public abstract string CollectionName { get; }


    public CacheRepository(IMongoOptions options)
    {
        var connectionString = $"mongodb://{options.User}:{options.Password}@{options.Host}:{options.Port}";

        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase(DatabaseName);
        _collection = _database.GetCollection<BsonEntity<T>>(CollectionName);
    }


    public virtual BsonEntity<T> Find(string id)
    {
        var entity = _collection.Find(entity => entity.Id == id).FirstOrDefault();

        return entity;
    }

    public virtual BsonEntity<T> FirstOrDefault()
    {
        var entry = FirstOrDefault(entry => true);

        return entry;
    }

    public virtual BsonEntity<T> FirstOrDefault(Expression<Func<BsonEntity<T>, bool>> predicate)
    {
        var entry = _collection.Find(predicate).FirstOrDefault();

        return entry;
    }

    public virtual IEnumerable<BsonEntity<T>> Where(Expression<Func<BsonEntity<T>, bool>> predicate)
    {
        var entry = _collection.Find(predicate).ToList();

        return entry;
    }

    public virtual async Task<IEnumerable<BsonEntity<T>>> WhereAsync(Expression<Func<BsonEntity<T>, bool>> predicate)
    {
        var entry = await _collection.Find(predicate).ToListAsync();

        return entry;
    }

    public virtual bool Any()
    {
        var any = _collection.Find(entry => true).Any();

        return any;
    }

    public virtual bool Any(Expression<Func<BsonEntity<T>, bool>> predicate)
    {
        var any = _collection.Find(predicate).Any();

        return any;
    }

    public virtual string Add(T document)
    {
        var entity = new BsonEntity<T>(document);

        _collection.InsertOne(entity);

        return entity.Id;
    }

    public virtual async Task<string> AddAsync(T document)
    {
        var entity = new BsonEntity<T>(document);

        await _collection.InsertOneAsync(entity);

        return entity.Id;
    }

    public void Delete(string id)
    {
        _collection.FindOneAndDelete(entity => entity.Id == id);
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.FindOneAndDeleteAsync(entity => entity.Id == id);
    }

    public void Update(string id, T document)
    {
        var update = Builders<BsonEntity<T>>.Update.Set(item => item.Document, document);
        _collection.UpdateOne(item => item.Id == id, update);
    }

    public async Task UpdateAsync(string id, T document)
    {
        var update = Builders<BsonEntity<T>>.Update.Set(item => item.Document, document);
        await _collection.UpdateOneAsync(item => item.Id == id, update);
    }
}
