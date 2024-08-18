namespace Matchmaker.Sessions;

public sealed class MatchmakingService(
  MatchmakingConfig config,
  IMatchmakingStore store)
{
  public ValueTask Handle(IMatchRequest request)
  {
    return request switch
    {
      CreateRequest data => Create(data.User, data.Time),
      DeleteRequest data => TryDelete(data.UserId),
      _ => throw new ArgumentException($"Invalid: {request.GetType().FullName}")
    };
  }

  private async ValueTask Create(User user, DateTime currentTime)
  {
    store.AddOrUpdate(user);

    CancellationToken token = user.GetOrCreateCancellationToken();

    _ = Task.Delay(config.MaxWaitTime, token)
      .ContinueWith(async _ => await AddToActiveSession(user, DateTime.UtcNow),
        TaskContinuationOptions.OnlyOnRanToCompletion);

    List<User>? matchedUsers = MatchUsers(store.GetWaitingUsers(), currentTime);

    if (matchedUsers is null)
    {
      return;
    }

    await Task.WhenAll(matchedUsers
      .Select(matchedUser => AddToActiveSession(matchedUser, currentTime)));
  }

  private List<User>? MatchUsers(IEnumerable<User> waitingUsers, DateTime time)
  {
    List<User> matchedUsers = [];

    foreach (User user in waitingUsers
               .OrderBy(user => user.QueueTime)
               .TakeWhile(_ => matchedUsers.Count < config.MaxUsersPerSession))
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

  private Task AddToActiveSession(User user, DateTime time)
  {
    user.TryCancel();

    store.TryRemoveUser(user.Id, out _);

    Session session = GetOrCreateSession(user, time);

    user.SessionId = session.Id;

    return store.Save(user.Id, session.Id, config.MaxRetainTime);
  }

  private Session GetOrCreateSession(User user, DateTime time)
  {
    store.RemoveFullSessions(config.MaxUsersPerSession);

    Session? session = store.GetSessions()
      .FirstOrDefault(session => config.MatchCriteria
        .Any(c => c.Matches(user, session.Users, time)));

    if (session is not null)
    {
      return session;
    }

    Session newSession = new(SessionId.Create(), []);

    store.Add(newSession);

    return newSession;
  }

  private ValueTask TryDelete(UserId userId)
  {
    if (store.TryRemoveUser(userId, out User? _))
    {
      return ValueTask.CompletedTask;
    }

    User user = new(userId, TimeSpan.Zero, DateTime.UtcNow);

    foreach (Session session in store.GetSessions())
    {
      if (session.Contains(user))
      {
        session.Remove(user);
      }
    }

    return ValueTask.CompletedTask;
  }
}
