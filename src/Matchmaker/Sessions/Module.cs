namespace Matchmaker.Sessions;

using Create;
using Delete;
using Get;
using System.Threading.Channels;

public static class Module
{
  public static void MapSessionEndpoints(this IEndpoints endpoints)
  {
    endpoints.MapCreateSession();
    endpoints.MapGetSession();
    endpoints.MapDeleteSession();
  }

  public static void ConfigureSessionModule(this IServices services)
  {
    const int minUsersPerSession = 2;
    const int maxUsersPerSession = 10;

    TimeSpan maxWaitTime = TimeSpan.FromSeconds(6);
    TimeSpan maxRetainTime = TimeSpan.FromMinutes(10);

    TimeSpan latencyRange = TimeSpan.FromMilliseconds(120);
    TimeSpan maxQueueWaitTime = maxWaitTime.Subtract(TimeSpan.FromSeconds(2));

    services.AddSingleton(new MatchmakingConfig(
      minUsersPerSession,
      maxUsersPerSession,
      maxWaitTime,
      maxRetainTime,
      [
        new MatchedUsersCountCriterion(0),
        new LatencyCriterion(latencyRange),
        new QueueTimeCriterion(maxQueueWaitTime)
      ]));

    services.AddSingleton<IMatchmakingStore, MatchmakingStore>();
    services.AddSingleton<MatchmakingService>();

    Channel<IMatchRequest> channel = Channel.CreateUnbounded<IMatchRequest>(
      new UnboundedChannelOptions
      {
        SingleReader = true, SingleWriter = false
      });

    services.AddSingleton(channel.Reader);
    services.AddSingleton(channel.Writer);

    services.AddHostedService<HostedService>();

    services.ConfigureCreateSession();
    services.ConfigureGetSession();
    services.ConfigureDeleteSession();
  }
}
