namespace Matchmaker;

using Microsoft.AspNetCore.ResponseCompression;

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

    services.AddResponseCompression(options =>
    {
      options.EnableForHttps = true;
      options.Providers.Add<BrotliCompressionProvider>();
      options.Providers.Add<GzipCompressionProvider>();
    });
  }

  public static void MapMiddlewares(this WebApplication app)
  {
    app.UseResponseCompression();
  }
}
