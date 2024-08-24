namespace Matchmaker.Sessions.Delete;

public static class Route
{
  public static void MapDeleteSession(this IEndpoints endpoints)
  {
    endpoints.MapDelete("/users/{userId:long}/session", (
          HttpContext context,
          long userId,
          IController controller) =>
        controller.Handle(context, userId))
      .WithName("DeleteUserSession")
      .Produces(Status204NoContent)
      .Produces(Status404NotFound)
      .Produces(Status400BadRequest)
      .WithTags("Session");
  }
}
