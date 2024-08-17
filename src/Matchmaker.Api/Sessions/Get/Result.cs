namespace Matchmaker.Sessions.Get;

public interface IResult;

public sealed record GetResult(SessionDto Session) : IResult;

public sealed record NotFoundResult : IResult;
