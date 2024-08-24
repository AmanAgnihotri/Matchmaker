namespace Matchmaker.Sessions;

using System.Threading.Channels;

public class HostedService(IServiceProvider provider) : BackgroundService
{
  private readonly ChannelReader<IMatchRequest> _queue =
    provider.GetRequiredService<ChannelReader<IMatchRequest>>();

  private readonly MatchmakingService _service =
    provider.GetRequiredService<MatchmakingService>();

  private readonly IDb _db = provider.GetRequiredService<IDb>();

  private readonly TimeSpan _maxWaitTime = TimeSpan.FromSeconds(2).Add(
    provider.GetRequiredService<MatchmakingConfig>().MaxWaitTime);

  private readonly TimeSpan _retainTime = TimeSpan.FromMinutes(10);

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      Task<IMatchRequest> readTask = _queue.ReadAsync(stoppingToken).AsTask();
      Task delayTask = Task.Delay(_maxWaitTime, stoppingToken);

      Task completedTask = await Task.WhenAny(readTask, delayTask);

      if (completedTask == readTask)
      {
        IMatchRequest request = await readTask;

        switch (request)
        {
          case CreateRequest data:
            {
              _service.AddUser(data.User);

              await TryCreateSessions(data.Time);
            }
            break;
          case DeleteRequest data:
            _service.RemoveUser(data.UserId);
            break;
        }
      }
      else
      {
        await TryCreateSessions(DateTime.UtcNow, true);
      }
    }

    if (_service.GetWaitingUsersCount() > 0)
    {
      await TryCreateSessions(DateTime.UtcNow, true);
    }
  }

  private async Task TryCreateSessions(DateTime time, bool forced = false)
  {
    List<IEvent> events = _service.TryCreateSessions(time, forced).ToList();

    if (events.Count > 0)
    {
      await Task.WhenAll(events
        .OfType<SessionCreated>()
        .Select(info =>
        {
          string key = info.User.Id.ToString();
          string value = info.Session.Id.ToString();

          return _db.SetString(key, value, _retainTime);
        }));
    }
  }
}
