namespace Matchmaker;

public sealed record User(
  UserId Id,
  TimeSpan Latency,
  DateTime QueueTime)
{
  private CancellationTokenSource? _source;

  public CancellationToken GetOrCreateCancellationToken()
  {
    if (_source is not null)
    {
      return _source.Token;
    }

    _source = new CancellationTokenSource();

    return _source.Token;
  }

  public void TryCancel()
  {
    if (_source is null)
    {
      return;
    }

    _source.Cancel();
    _source.Dispose();

    _source = null;
  }

  public bool Equals(User? other)
  {
    if (other is null)
    {
      return false;
    }

    return ReferenceEquals(this, other) || Id.Equals(other.Id);
  }

  public override int GetHashCode()
  {
    return Id.GetHashCode();
  }
}
