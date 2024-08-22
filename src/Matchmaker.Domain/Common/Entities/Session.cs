namespace Matchmaker;

public sealed record Session(
  SessionId Id,
  List<User> Users);
