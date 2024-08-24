using Matchmaker;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ConfigureApp();

await using WebApplication app = builder.Build();

app.MapRoot();
app.MapMiddlewares();
app.MapEndpoints();

await app.RunAsync();
