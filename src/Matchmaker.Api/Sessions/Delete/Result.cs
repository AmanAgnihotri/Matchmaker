namespace Matchmaker.Sessions.Delete;

public interface IResult;

public sealed record DeleteResult : IResult;

public sealed record NotFoundResult : IResult;
