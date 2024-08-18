namespace Matchmaker.Sessions;

using System.Threading.Channels;

public sealed class HostedService(IServiceProvider provider) : BackgroundService
{
  private readonly IHostApplicationLifetime _lifetime =
    provider.GetRequiredService<IHostApplicationLifetime>();

  private readonly ChannelReader<IMatchRequest> _queue =
    provider.GetRequiredService<ChannelReader<IMatchRequest>>();

  private readonly MatchmakingService _service =
    provider.GetRequiredService<MatchmakingService>();

  private readonly TimeSpan _maxWaitTime = TimeSpan.FromSeconds(2).Add(
    provider.GetRequiredService<MatchmakingConfig>().MaxWaitTime);

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    await foreach (IMatchRequest request in _queue.ReadAllAsync(stoppingToken))
    {
      await _service.Handle(request);
    }
  }

  public override async Task StopAsync(CancellationToken cancellationToken)
  {
    _lifetime.StopApplication();

    await Task.Delay(_maxWaitTime, cancellationToken);

    await base.StopAsync(cancellationToken);
  }
}
