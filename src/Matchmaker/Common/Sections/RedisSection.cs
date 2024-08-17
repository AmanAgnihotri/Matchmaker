namespace Matchmaker;

public sealed record RedisSection
{
  public string? ConnectionString { get; init; }

  public bool IsValid()
  {
    return !string.IsNullOrWhiteSpace(ConnectionString);
  }
}
