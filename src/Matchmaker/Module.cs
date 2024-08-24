namespace Matchmaker;

public static class Module
{
  public static void MapRoot(this WebApplication app)
  {
    string result = $"Matchmaker API! ({app.Environment.EnvironmentName})";

    app.MapGet("/", () => result);
  }
}
