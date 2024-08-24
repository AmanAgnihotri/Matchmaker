namespace Matchmaker.Sessions.Create;

using System.Threading.Channels;

public interface IHandler
{
  Task<IResult> Handle(Command command);
}

public sealed class Handler(
  IStore store,
  ChannelWriter<IMatchRequest> queue) : IHandler
{
  private readonly IResult _result = new CreateResult();

  public async Task<IResult> Handle(Command command)
  {
    SessionId? sessionId = await store.GetSession(command.UserId);

    if (sessionId is not null)
    {
      return new ConflictResult(sessionId.Value);
    }

    User user = new(command.UserId, command.Latency, command.CreateTime);

    await queue.WriteAsync(new CreateRequest(user, command.CreateTime));

    return _result;
  }
}
