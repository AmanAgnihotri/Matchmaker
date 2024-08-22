namespace Matchmaker;

public sealed record User(
  UserId Id,
  TimeSpan Latency,
  DateTime QueueTime);
