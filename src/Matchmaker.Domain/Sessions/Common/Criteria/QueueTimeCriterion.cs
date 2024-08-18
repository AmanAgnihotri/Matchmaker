namespace Matchmaker.Sessions;

public sealed class QueueTimeCriterion(TimeSpan maxWaitTime) : IMatchCriterion
{
  public bool Matches(User user, List<User> matchedUsers, DateTime currentTime)
  {
    return currentTime - user.QueueTime >= maxWaitTime;
  }
}
