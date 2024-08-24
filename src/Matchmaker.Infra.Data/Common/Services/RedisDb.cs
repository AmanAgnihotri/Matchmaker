namespace Matchmaker;

using StackExchange.Redis;

public sealed class RedisDb(RedisConfig config) : IDb
{
  private readonly ConnectionMultiplexer _mux =
    ConnectionMultiplexer.Connect(config.ConnectionString);

  public async Task<string?> GetString(string key)
  {
    try
    {
      RedisValue value = await _mux.GetDatabase().StringGetAsync(key);

      return value.IsNullOrEmpty ? null : value.ToString();
    }
    catch
    {
      return null;
    }
  }

  public Task<bool> SetString(string key, string value, TimeSpan? duration)
  {
    return _mux.GetDatabase().StringSetAsync(key, value, duration);
  }

  public Task<bool> DeleteString(string key)
  {
    return _mux.GetDatabase().KeyDeleteAsync(key);
  }
}
