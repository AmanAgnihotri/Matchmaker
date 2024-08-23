namespace Matchmaker.Sessions;

public interface IEvent;

public sealed record SessionCreated(Session Session, User User) : IEvent;
