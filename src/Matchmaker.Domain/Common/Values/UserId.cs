namespace Matchmaker;

public readonly record struct UserId
{
  private readonly long _value;

  public UserId()
  {
    throw new ArgumentNullException($"{nameof(UserId)} cannot be null.");
  }

  private UserId(long value, bool validate)
  {
    if (validate && !Id.IsValid(value))
    {
      throw new ArgumentOutOfRangeException(nameof(value));
    }

    _value = value;
  }

  public static bool TryParse(long value, out UserId userId)
  {
    if (Id.IsValid(value))
    {
      userId = new UserId(value, false);

      return true;
    }

    userId = default;

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

  public bool Equals(UserId other)
  {
    return _value == other._value;
  }
}
