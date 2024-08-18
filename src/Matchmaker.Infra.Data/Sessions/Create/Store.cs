namespace Matchmaker.Sessions.Create;

public sealed class Store(IDbService db) : IStore
{
  public async ValueTask<SessionId?> GetSession(UserId userId)
  {
    try
    {
      RedisValue value = await db.Database.StringGetAsync(userId.ToString());

      if (value.IsNullOrEmpty)
      {
        return null;
      }

      return long.TryParse(value, out long result)
        ? new SessionId(result)
        : null;
    }
    catch
    {
      return null;
    }
  }
}
