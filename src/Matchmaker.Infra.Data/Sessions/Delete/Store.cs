namespace Matchmaker.Sessions.Delete;

public sealed class Store(IDb db) : IStore
{
  public async Task<bool> DeleteSession(UserId userId)
  {
    try
    {
      return await db.DeleteString(userId.ToString());
    }
    catch
    {
      return false;
    }
  }
}
