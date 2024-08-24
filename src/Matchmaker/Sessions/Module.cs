namespace Matchmaker.Sessions;

using Delete;
using Get;

public static class Module
{
  public static void MapSessionEndpoints(this IEndpoints endpoints)
  {
    endpoints.MapGetSession();
    endpoints.MapDeleteSession();
  }

  public static void ConfigureSessionModule(this IServices services)
  {
    services.ConfigureGetSession();
    services.ConfigureDeleteSession();
  }
}
