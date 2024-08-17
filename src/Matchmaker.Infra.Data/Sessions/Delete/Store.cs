namespace Matchmaker.Sessions.Delete;

public sealed class Store(IDbService db) : IStore
{
  public async Task<bool> DeleteSession(UserId userId)
  {
    try
    {
      RedisValue value =
        await db.Database.StringGetDeleteAsync(userId.ToString());

      return !value.IsNullOrEmpty && long.TryParse(value, out long _);
    }
    catch
    {
      return false;
    }
  }
}
