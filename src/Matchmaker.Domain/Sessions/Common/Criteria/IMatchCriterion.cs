namespace Matchmaker.Sessions;

public interface IMatchCriterion
{
  // Can the user match with the matched users at current time?
  bool Matches(User user, List<User> matchedUsers, DateTime currentTime);
}
