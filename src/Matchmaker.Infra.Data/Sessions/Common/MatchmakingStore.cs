namespace Matchmaker.Sessions;

using System.Collections.Concurrent;

public sealed class MatchmakingStore(IDbService db) : IMatchmakingStore
{
  private readonly ConcurrentDictionary<UserId, User> _waitingUsers = [];
  private readonly ConcurrentDictionary<SessionId, Session> _sessions = [];

  public void AddOrUpdate(User user)
  {
    _waitingUsers.AddOrUpdate(user.Id, user, (_, existingUser) =>
    {
      existingUser.TryCancel();

      return user;
    });
  }

  public bool TryRemoveUser(UserId userId, out User? user)
  {
    return _waitingUsers.TryRemove(userId, out user);
  }

  public IEnumerable<User> GetWaitingUsers()
  {
    return _waitingUsers.Values;
  }

  public void Add(Session session)
  {
    _sessions.TryAdd(session.Id, session);
  }

  public void RemoveSession(SessionId sessionId)
  {
    _sessions.TryRemove(sessionId, out _);
  }

  public IEnumerable<Session> GetSessions()
  {
    return _sessions.Values;
  }

  public void RemoveFullSessions(int maxUsersPerSession)
  {
    foreach (Session session in GetSessions())
    {
      if (session.Users.Count == maxUsersPerSession)
      {
        RemoveSession(session.Id);
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
