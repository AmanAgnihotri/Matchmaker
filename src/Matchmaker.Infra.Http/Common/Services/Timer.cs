namespace Matchmaker;

public interface ITimer
{
  DateTime UtcNow { get; }
}

public sealed class Timer : ITimer
{
  public DateTime UtcNow => DateTime.UtcNow;
}
