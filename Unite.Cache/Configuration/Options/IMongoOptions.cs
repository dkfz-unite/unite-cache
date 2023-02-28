namespace Unite.Cache.Configuration.Options;

public interface IMongoOptions
{
    string Host { get; }
    string Port { get; }
    string User { get; }
    string Password { get; }
}
