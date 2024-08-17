namespace Matchmaker;

public static class Module
{
  public static void MapRoot(this WebApplication app)
  {
    string result = $"Matchmaker API! ({app.Environment.EnvironmentName})";

    app.MapGet("/", () => result);
  }

  public static void ConfigureApp(this WebApplicationBuilder builder)
  {
    IServices services = builder.Services;
    IConfiguration configuration = builder.Configuration;

    services.AddSingleton<ITimer, Timer>();
  }
}
