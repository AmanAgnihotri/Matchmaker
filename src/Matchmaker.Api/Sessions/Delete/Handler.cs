namespace Matchmaker.Sessions.Delete;

public interface IHandler
{
  Task<IResult> Handle(Command command);
}

public sealed class Handler(IStore store) : IHandler
{
  public async Task<IResult> Handle(Command command)
  {
    bool deleted = await store.DeleteSession(command.UserId);

    return deleted ? new DeleteResult() : new NotFoundResult();
  }
}
