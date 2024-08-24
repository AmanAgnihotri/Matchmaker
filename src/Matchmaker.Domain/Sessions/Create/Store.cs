namespace Matchmaker.Sessions.Create;

public interface IStore
{
  ValueTask<SessionId?> GetSession(UserId userId);
}
