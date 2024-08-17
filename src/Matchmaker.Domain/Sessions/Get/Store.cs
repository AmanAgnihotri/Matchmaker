namespace Matchmaker.Sessions.Get;

public interface IStore
{
  Task<SessionId?> GetSession(UserId userId);
}
