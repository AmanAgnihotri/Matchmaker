namespace Matchmaker.Sessions;

public sealed class MatchmakingStore(IDbService db) : IMatchmakingStore
{
  public void AddOrUpdate(User user)
  {
    db.InMemoryStore.AddOrUpdateUser(user);
  }

  public bool TryRemoveUser(UserId userId, out User? user)
  {
    return db.InMemoryStore.TryRemoveUser(userId, out user);
  }

  public IEnumerable<User> GetWaitingUsers()
  {
    return db.InMemoryStore.GetWaitingUsers();
  }

  public void Add(Session session)
  {
    db.InMemoryStore.TryAdd(session);
  }

  public Session? GetSession(SessionId sessionId)
  {
    return db.InMemoryStore.GetSessionOrDefault(sessionId);
  }

  public IEnumerable<Session> GetSessions()
  {
    return db.InMemoryStore.GetSessions();
  }

  public void RemoveFullSessions(int maxUsersPerSession)
  {
    foreach (Session session in db.InMemoryStore.GetSessions())
    {
      if (session.Users.Count == maxUsersPerSession)
      {
        db.InMemoryStore.TryRemoveSession(session.Id);
      }
    }
  }

  public Task Save(
    UserId userId,
    SessionId sessionId,
    TimeSpan maxRetainTime)
  {
    try
    {
      return db.Database.StringSetAsync(
        userId.ToString(), sessionId.ToString(), maxRetainTime);
    }
    catch
    {
      return Task.CompletedTask;
    }
  }
}
