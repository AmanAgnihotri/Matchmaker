namespace Matchmaker;

public readonly record struct SessionId
{
  private readonly long _value;

  public SessionId()
  {
    throw new ArgumentNullException($"{nameof(SessionId)} cannot be null.");
  }

  private SessionId(long value, bool validate)
  {
    if (validate && !Id.IsValid(value))
    {
      throw new ArgumentOutOfRangeException(nameof(value));
    }

    _value = value;
  }

  public static SessionId Create()
  {
    return new SessionId(Id.Create(), false);
  }

  public static bool TryParse(string? value, out SessionId? sessionId)
  {
    if (!string.IsNullOrWhiteSpace(value))
    {
      if (long.TryParse(value, out long result))
      {
        sessionId = new SessionId(result, false);

        return true;
      }
    }

    sessionId = null;

    return false;
  }

  public override string ToString()
  {
    return _value.ToString();
  }

  public override int GetHashCode()
  {
    return _value.GetHashCode();
  }

  public bool Equals(SessionId other)
  {
    return _value == other._value;
  }

  public static implicit operator long(SessionId sessionId)
  {
    return sessionId._value;
  }
}
