namespace Matchmaker.Sessions;

public sealed class MatchmakingState(
  Dictionary<UserId, User> waitingUsers,
  Dictionary<SessionId, Session> activeSessions)
{
  public bool AddUser(User user)
  {
    return waitingUsers.TryAdd(user.Id, user);
  }

  public User? TryGetUser(UserId userId)
  {
    return waitingUsers.GetValueOrDefault(userId);
  }

  public bool RemoveUser(UserId userId)
  {
    return waitingUsers.Remove(userId);
  }

  public bool AddSession(Session session)
  {
    return activeSessions.TryAdd(session.Id, session);
  }

  public IEnumerable<User> GetWaitingUsers()
  {
    return waitingUsers.Values;
  }

  public IEnumerable<Session> GetActiveSessions(int maxUsersPerSession)
  {
    TryCleanup(maxUsersPerSession);

    return activeSessions.Values;
  }

  private void TryCleanup(int maxUsersPerSession)
  {
    if (!activeSessions.Any(v => IsEmptyOrFullSession(v.Value.Users.Count)))
    {
      return;
    }

    List<SessionId> sessionsToRemove = activeSessions
      .Where(v => IsEmptyOrFullSession(v.Value.Users.Count))
      .Select(v => v.Key)
      .ToList();

    foreach (SessionId sessionId in sessionsToRemove)
    {
      activeSessions.Remove(sessionId);
    }

    return;

    bool IsEmptyOrFullSession(int userCount)
    {
      return userCount == 0 || userCount >= maxUsersPerSession;
    }
  }
}
