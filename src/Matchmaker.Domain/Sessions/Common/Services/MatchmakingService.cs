namespace Matchmaker.Sessions;

public sealed class MatchmakingService(
  MatchmakingConfig config,
  MatchmakingState state)
{
  public bool AddUser(User user)
  {
    return state.AddUser(user);
  }

  public void RemoveUsers(IEnumerable<User> users)
  {
    foreach (User user in users)
    {
      state.RemoveUser(user.Id);
    }
  }

  public List<User>? MatchUsers(DateTime time)
  {
    List<User> matchedUsers = [];

    foreach (User user in state.GetWaitingUsers()
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
}
