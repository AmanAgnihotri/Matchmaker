namespace Matchmaker.Sessions;

public interface IMatchCriterion
{
  bool Matches(User user, List<User> matchedUsers, DateTime currentTime);
}
