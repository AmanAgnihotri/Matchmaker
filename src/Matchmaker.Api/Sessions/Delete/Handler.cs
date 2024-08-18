namespace Matchmaker.Sessions.Delete;

using System.Threading.Channels;

public interface IHandler
{
  Task<IResult> Handle(Command command);
}

public sealed class Handler(
  IStore store,
  ChannelWriter<IMatchRequest> queue) : IHandler
{
  public async Task<IResult> Handle(Command command)
  {
    bool deleted = await store.DeleteSession(command.UserId);

    await queue.WriteAsync(new DeleteRequest(command.UserId));

    return deleted ? new DeleteResult() : new NotFoundResult();
  }
}
