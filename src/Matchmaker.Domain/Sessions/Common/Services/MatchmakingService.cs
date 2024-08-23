namespace Matchmaker.Sessions;

public sealed class MatchmakingService(
  MatchmakingConfig config,
  MatchmakingState state)
{
  public bool AddUser(User user)
  {
    return state.AddUser(user);
  }

  public IEnumerable<IEvent> TryCreateSessions(DateTime time)
  {
    while (MatchUsers(time) is { } matchedUsers)
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

  public List<User>? MatchUsers(DateTime time)
  {
    if (state.GetWaitingUsersCount() < config.MinUsersPerSession)
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

    return matchedUsers.Count >= config.MinUsersPerSession
      ? matchedUsers
      : null;
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
