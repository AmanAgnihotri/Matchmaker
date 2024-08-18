namespace Matchmaker.Sessions;

public interface IMatchmakingStore
{
  void AddOrUpdate(User user);

  bool TryRemoveUser(UserId userId, out User? user);

  IEnumerable<User> GetWaitingUsers();

  void Add(Session session);

  Session? GetSession(SessionId sessionId);

  void RemoveSession(SessionId sessionId);

  IEnumerable<Session> GetSessions();

  void RemoveFullSessions(int maxUsersPerSession);

  Task Save(UserId userId, SessionId sessionId, TimeSpan maxRetainTime);
}
