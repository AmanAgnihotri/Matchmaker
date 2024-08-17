namespace Matchmaker;

public readonly record struct UserId
{
  private readonly long _value;

  public UserId(long value)
  {
    _value = value;
  }

  public UserId(long value, bool validate)
  {
    if (validate && !Id.IsValid(value))
    {
      throw new ArgumentOutOfRangeException(nameof(value));
    }

    _value = value;
  }

  public static UserId Create()
  {
    return new UserId(Id.Create(), false);
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

  public static implicit operator string(UserId userId)
  {
    return userId.ToString();
  }
}
