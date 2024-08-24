namespace Matchmaker.Sessions.Create;

public interface IResult;

public sealed record CreateResult : IResult;

public sealed record ConflictResult(long SessionId) : IResult;
