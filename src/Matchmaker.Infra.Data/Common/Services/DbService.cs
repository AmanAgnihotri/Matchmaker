namespace Matchmaker;

public interface IDbService
{
  IDatabase Database { get; }
}

public sealed class DbService : IDbService
{
  public IDatabase Database { get; }

  public DbService(DbConfig config)
  {
    ConnectionMultiplexer connection =
      ConnectionMultiplexer.Connect(config.ConnectionString);

    Database = connection.GetDatabase();
  }
}
