namespace Matchmaker.Sessions.Get;

public interface IHandler
{
  Task<IResult> Handle(Query query);
}

public sealed class Handler(IStore store) : IHandler
{
  public async Task<IResult> Handle(Query query)
  {
    SessionId? sessionId = await store.GetSession(query.UserId);

    if (sessionId is null)
    {
      return new NotFoundResult();
    }

    return new GetResult(new SessionDto(sessionId.Value));
  }
}
