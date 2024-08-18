namespace Matchmaker;

using System.Collections.Concurrent;

public sealed class InMemoryStore
{
  private readonly ConcurrentDictionary<UserId, User> _waitingUsers = [];
  private readonly ConcurrentDictionary<SessionId, Session> _sessions = [];

  public void AddOrUpdateUser(User user)
  {
    _waitingUsers.AddOrUpdate(user.Id, user, (_, existingUser) =>
    {
      existingUser.TryCancel();

      return user;
    });
  }

  public SessionId? TryGetSessionId(UserId userId)
  {
    return _waitingUsers.TryGetValue(userId, out User? user)
      ? user.SessionId
      : null;
  }

  public bool TryRemoveUser(UserId userId, out User? user)
  {
    return _waitingUsers.TryRemove(userId, out user);
  }

  public IEnumerable<User> GetWaitingUsers()
  {
    return _waitingUsers.Values;
  }

  public void TryAdd(Session session)
  {
    _sessions.TryAdd(session.Id, session);
  }

  public Session? GetSessionOrDefault(SessionId sessionId)
  {
    return _sessions.GetValueOrDefault(sessionId);
  }

  public IEnumerable<Session> GetSessions()
  {
    return _sessions.Values;
  }

  public void TryRemoveSession(SessionId sessionId)
  {
    _sessions.TryRemove(sessionId, out _);
  }
}
