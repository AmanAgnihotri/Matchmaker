namespace Matchmaker;

public sealed record Session(
  SessionId Id,
  List<User> Users)
{
  public void Remove(User user)
  {
    Users.Remove(user);
  }
}
