namespace Matchmaker.Sessions.Get;

public interface IStore
{
  ValueTask<SessionId?> GetSession(UserId userId);
}
