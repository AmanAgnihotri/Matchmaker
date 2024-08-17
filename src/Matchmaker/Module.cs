namespace Matchmaker;

using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.ResponseCompression;
using System.Text.Json;
using System.Text.Json.Serialization;

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

    services.ConfigureJsonSerializer();

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

  private static void ConfigureJsonSerializer(this IServices services)
  {
    services.Configure<JsonOptions>(options =>
    {
      options.SerializerOptions.PropertyNamingPolicy =
        JsonNamingPolicy.CamelCase;

      options.SerializerOptions.DefaultIgnoreCondition =
        JsonIgnoreCondition.WhenWritingNull;

      options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });
  }
}
