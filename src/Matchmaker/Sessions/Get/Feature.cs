namespace Matchmaker.Sessions.Get;

public static class Feature
{
  public static void ConfigureGetSession(this IServices services)
  {
    services
      .AddSingleton<IController, Controller>()
      .AddSingleton<IHandler, Handler>()
      .AddSingleton<IStore, Store>();
  }
}
