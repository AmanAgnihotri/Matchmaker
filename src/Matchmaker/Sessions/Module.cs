namespace Matchmaker.Sessions;

using Get;

public static class Module
{
  public static void MapSessionEndpoints(this IEndpoints endpoints)
  {
    endpoints.MapGetSession();
  }

  public static void ConfigureSessionModule(this IServices services)
  {
    services.ConfigureGetSession();
  }
}
