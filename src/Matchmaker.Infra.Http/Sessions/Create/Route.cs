namespace Matchmaker.Sessions.Create;

public static class Route
{
  public static void MapCreateSession(this IEndpoints endpoints)
  {
    endpoints.MapPost("/users/{userId:long}/sessions", (
          HttpContext context,
          long userId,
          Request? request,
          IController controller) =>
        controller.Handle(context, userId, request))
      .WithName("CreateUserSession")
      .Produces(Status204NoContent)
      .Produces<SessionDto>(Status409Conflict)
      .Produces(Status400BadRequest)
      .WithTags("Session");
  }
}
