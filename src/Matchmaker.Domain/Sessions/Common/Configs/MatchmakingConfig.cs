namespace Matchmaker.Sessions;

public sealed record MatchmakingConfig(
  int MinUsersPerSession,
  int MaxUsersPerSession,
  TimeSpan MaxWaitTime,
  TimeSpan MaxRetainTime,
  IReadOnlyList<IMatchCriterion> MatchCriteria);
