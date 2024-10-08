namespace Matchmaker.Sessions;

public sealed class MatchmakingService(
  MatchmakingConfig config,
  MatchmakingState state)
{
  public bool AddUser(User user)
  {
    return state.AddUser(user);
  }

  public IEnumerable<IEvent> TryCreateSessions(
    DateTime time,
    bool forced = false)
  {
    while (MatchUsers(time, forced) is { } matchedUsers)
    {
      foreach (User user in matchedUsers)
      {
        Session session = GetOrCreateSession(user, time);

        session.Users.Add(user);

        yield return new SessionCreated(session, user);
      }

      RemoveUsersFromQueue(matchedUsers);
    }
  }

  public int GetWaitingUsersCount()
  {
    return state.GetWaitingUsersCount();
  }

  public List<User>? MatchUsers(DateTime time, bool forced = false)
  {
    int count = GetWaitingUsersCount();

    if (count == 0 || (count == 1 && !forced))
    {
      return null;
    }

    List<User> matchedUsers = [];

    foreach (User user in state.GetWaitingUsers()
               .OrderBy(user => user.QueueTime)
               .TakeWhile(u => matchedUsers.Count < config.MaxUsersPerSession ||
                               time - u.QueueTime >= config.MaxWaitTime))
    {
      if (config.MatchCriteria.Any(c => c.Matches(user, matchedUsers, time)))
      {
        matchedUsers.Add(user);
      }
    }

    if (forced && matchedUsers.Count > 0)
    {
      return matchedUsers;
    }

    return matchedUsers.Count >= config.MinUsersPerSession
      ? matchedUsers
      : null;
  }

  public bool RemoveUser(UserId userId)
  {
    if (state.RemoveUser(userId))
    {
      return true;
    }

    IEnumerable<Session> sessions =
      state.GetActiveSessions(config.MaxUsersPerSession);

    foreach (Session session in sessions)
    {
      User? user = session.Users.FirstOrDefault(user => user.Id == userId);

      if (user is null)
      {
        continue;
      }

      session.Users.Remove(user);

      return true;
    }

    return false;
  }

  public void RemoveUsersFromQueue(IEnumerable<User> users)
  {
    foreach (User user in users)
    {
      state.RemoveUser(user.Id);
    }
  }

  private Session GetOrCreateSession(User user, DateTime time)
  {
    Session? session = state.GetActiveSessions(config.MaxUsersPerSession)
      .FirstOrDefault(session => config.MatchCriteria
        .Any(c => c.Matches(user, session.Users, time)));

    return session ?? CreateSession();
  }

  private Session CreateSession()
  {
    Session newSession = new(SessionId.Create(), []);

    state.AddSession(newSession);

    return newSession;
  }
}
