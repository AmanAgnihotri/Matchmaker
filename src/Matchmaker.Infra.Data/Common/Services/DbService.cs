namespace Matchmaker;

public interface IDbService
{
  IConnectionMultiplexer Connection { get; }

  IDatabase Database { get; }

  InMemoryStore InMemoryStore { get; }
}

public sealed class DbService : IDbService
{
  public IConnectionMultiplexer Connection { get; }

  public IDatabase Database { get; }

  public InMemoryStore InMemoryStore { get; }

  public DbService(DbConfig config)
  {
    Connection = ConnectionMultiplexer.Connect(config.ConnectionString);

    Database = Connection.GetDatabase();

    InMemoryStore = new InMemoryStore();
  }
}
