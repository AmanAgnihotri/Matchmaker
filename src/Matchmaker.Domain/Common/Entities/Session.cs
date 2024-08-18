namespace Matchmaker;

public sealed record Session(
  SessionId Id,
  List<User> Users)
{
  public bool Contains(User user)
  {
    return Users.Contains(user);
  }

  public void Remove(User user)
  {
    Users.Remove(user);
  }
}
