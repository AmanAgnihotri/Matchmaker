namespace Matchmaker;

public interface IDbService
{
  IConnectionMultiplexer Connection { get; }

  IDatabase Database { get; }
}

public sealed class DbService : IDbService
{
  public IConnectionMultiplexer Connection { get; }

  public IDatabase Database { get; }

  public DbService(DbConfig config)
  {
    Connection = ConnectionMultiplexer.Connect(config.ConnectionString);

    Database = Connection.GetDatabase();
  }
}
