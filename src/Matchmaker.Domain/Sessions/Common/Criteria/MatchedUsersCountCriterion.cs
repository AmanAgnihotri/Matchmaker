namespace Matchmaker.Sessions;

public sealed class MatchedUsersCountCriterion(int count) : IMatchCriterion
{
  public bool Matches(User user, List<User> matchedUsers, DateTime currentTime)
  {
    return matchedUsers.Count == count;
  }
}
