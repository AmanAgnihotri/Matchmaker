namespace Matchmaker;

public readonly record struct SessionId
{
  private readonly long _value;

  public SessionId(long value)
  {
    _value = value;
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

  public static implicit operator string(SessionId sessionId)
  {
    return sessionId.ToString();
  }
}
