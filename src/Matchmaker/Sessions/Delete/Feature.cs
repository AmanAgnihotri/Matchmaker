namespace Matchmaker.Sessions.Delete;

public static class Feature
{
  public static void ConfigureDeleteSession(this IServices services)
  {
    services
      .AddSingleton<IController, Controller>()
      .AddSingleton<IHandler, Handler>()
      .AddSingleton<IStore, Store>();
  }
}
