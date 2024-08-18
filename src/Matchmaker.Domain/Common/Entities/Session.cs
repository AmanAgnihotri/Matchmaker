namespace Matchmaker;

public sealed record Session(
  SessionId Id,
  List<User> Users)
{
  public bool Contains(UserId userId)
  {
    return Users.Any(user => user.Id == userId);
  }

  public void Remove(UserId userId)
  {
    Users.RemoveAll(user => user.Id == userId);
  }
}
