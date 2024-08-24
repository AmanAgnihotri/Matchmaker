namespace Matchmaker.Sessions.Delete;

public interface IController
{
  Task Handle(HttpContext context, long userIdValue);
}

public sealed class Controller(IHandler handler) : IController
{
  public async Task Handle(HttpContext context, long userIdValue)
  {
    Command? command = NewCommand(userIdValue);

    if (command is not null)
    {
      IResult result = await handler.Handle(command.Value);

      await (result switch
      {
        DeleteResult => context.Status(Status204NoContent),
        NotFoundResult => context.Status(Status404NotFound),
        _ => context.Status(Status501NotImplemented)
      });
    }
    else
    {
      await context.Status(Status400BadRequest);
    }
  }

  private static Command? NewCommand(long userIdValue)
  {
    return UserId.TryParse(userIdValue, out UserId userId)
      ? new Command(userId)
      : null;
  }
}
