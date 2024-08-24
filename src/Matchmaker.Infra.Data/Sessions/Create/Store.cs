namespace Matchmaker.Sessions.Create;

public sealed class Store(IDb db) : IStore
{
  public async ValueTask<SessionId?> GetSession(UserId userId)
  {
    try
    {
      string? value = await db.GetString(userId.ToString());

      return SessionId.TryParse(value, out SessionId? result) ? result : null;
    }
    catch
    {
      return null;
    }
  }
}
