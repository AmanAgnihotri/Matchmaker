namespace Matchmaker.Sessions.Get;

public interface IResult;

public sealed record GetResult(long SessionId) : IResult;

public sealed record NotFoundResult : IResult;
