namespace Matchmaker.Sessions.Get;

public static class Route
{
  public static void MapGetSession(this IEndpoints endpoints)
  {
    endpoints.MapGet("/users/{userId:long}/session", (
          HttpContext context,
          long userId,
          IController controller) =>
        controller.Handle(context, userId))
      .WithName("GetUserSession")
      .Produces<GetResult>()
      .Produces(Status404NotFound)
      .Produces(Status400BadRequest)
      .WithTags("Session");
  }
}
