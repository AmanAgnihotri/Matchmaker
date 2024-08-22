namespace Matchmaker.Sessions;

public sealed record MatchmakingConfig(
  int MinUsersPerSession,
  int MaxUsersPerSession,
  IReadOnlyList<IMatchCriterion> MatchCriteria);
