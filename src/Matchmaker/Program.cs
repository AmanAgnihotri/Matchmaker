WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

await using WebApplication app = builder.Build();

app.MapGet("/", () => "Ok!");

await app.RunAsync();
