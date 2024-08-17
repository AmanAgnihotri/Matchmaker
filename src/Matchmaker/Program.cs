using Matchmaker;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

await using WebApplication app = builder.Build();

app.MapRoot();

await app.RunAsync();
