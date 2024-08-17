namespace Matchmaker;

public static class HttpContextExtensions
{
  public static Task Ok<T>(this HttpContext context, T value)
  {
    context.Response.StatusCode = Status200OK;

    return context.Response.WriteAsJsonAsync(value);
  }

  public static Task Data<T>(this HttpContext context, T value, int statusCode)
  {
    context.Response.StatusCode = statusCode;

    return context.Response.WriteAsJsonAsync(value);
  }

  public static Task Status(this HttpContext context, int statusCode)
  {
    context.Response.StatusCode = statusCode;

    return Task.CompletedTask;
  }
}
