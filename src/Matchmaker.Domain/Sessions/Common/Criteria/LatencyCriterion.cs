namespace Matchmaker.Sessions;

public class LatencyCriterion(TimeSpan range) : IMatchCriterion
{
  public bool Matches(User user, List<User> matchedUsers, DateTime currentTime)
  {
    long totalTicks = matchedUsers.Sum(u => u.Latency.Ticks);

    long averageTicks = totalTicks / matchedUsers.Count;

    return Math.Abs(user.Latency.Ticks - averageTicks) <= range.Ticks;
  }
}
