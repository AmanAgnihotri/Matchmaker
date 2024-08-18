namespace Matchmaker.Sessions;

public interface IMatchRequest;

public sealed record CreateRequest(User User, DateTime Time) : IMatchRequest;

public sealed record DeleteRequest(UserId UserId) : IMatchRequest;
