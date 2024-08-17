namespace Matchmaker.Sessions.Delete;

public interface IStore
{
  Task<bool> DeleteSession(UserId userId);
}
