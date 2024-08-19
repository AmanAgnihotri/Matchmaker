namespace Matchmaker.Sessions.Get;

public interface IController
{
  Task Handle(HttpContext context, long userIdValue);
}

public sealed class Controller(IHandler handler) : IController
{
  public async Task Handle(HttpContext context, long userIdValue)
  {
    Query? query = NewQuery(userIdValue);

    if (query is not null)
    {
      IResult result = await handler.Handle(query.Value);

      await (result switch
      {
        GetResult data => context.Ok(data),
        NotFoundResult => context.Status(Status404NotFound),
        _ => context.Status(Status501NotImplemented)
      });
    }
    else
    {
      await context.Status(Status400BadRequest);
    }
  }

  private static Query? NewQuery(long userIdValue)
  {
    return UserId.TryParse(userIdValue, out UserId userId)
      ? new Query(userId)
      : null;
  }
}
