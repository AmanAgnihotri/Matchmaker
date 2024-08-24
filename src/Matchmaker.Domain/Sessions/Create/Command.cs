namespace Matchmaker.Sessions.Create;

public sealed record Command(
  UserId UserId,
  TimeSpan Latency,
  DateTime CreateTime);
