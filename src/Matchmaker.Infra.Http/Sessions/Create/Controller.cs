namespace Matchmaker.Sessions.Create;

public interface IController
{
  Task Handle(HttpContext context, long userIdValue, Request? request);
}

public sealed class Controller(ITimer timer, IHandler handler) : IController
{
  public async Task Handle(
    HttpContext context,
    long userIdValue,
    Request? request)
  {
    Command? command = NewCommand(userIdValue, request);

    if (command is not null)
    {
      IResult result = await handler.Handle(command);

      await (result switch
      {
        CreateResult => context.Status(Status204NoContent),
        ConflictResult data => context.Data(data, Status409Conflict),
        _ => context.Status(Status501NotImplemented)
      });
    }
    else
    {
      await context.Status(Status400BadRequest);
    }
  }

  private Command? NewCommand(long userIdValue, Request? request)
  {
    if (request is null)
    {
      return default;
    }

    if (request.LatencyInMilliseconds is null or < 0 or >= 1000)
    {
      return default;
    }

    TimeSpan latency =
      TimeSpan.FromMilliseconds(request.LatencyInMilliseconds.Value);

    return UserId.TryParse(userIdValue, out UserId userId)
      ? new Command(userId, latency, timer.UtcNow)
      : null;
  }
}
