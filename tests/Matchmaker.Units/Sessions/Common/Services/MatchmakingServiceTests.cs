namespace Matchmaker.Sessions;

public sealed class MatchmakingServiceTests
{
  [Fact]
  public void TestAddUser()
  {
    Assert.True(UserId.TryParse(Id.Create(), out UserId userId));

    User user = new(userId, TimeSpan.FromMilliseconds(50), DateTime.UtcNow);

    MatchmakingConfig config = new(2, 10, []);
    MatchmakingState state = new([], []);

    MatchmakingService service = new(config, state);

    Assert.True(service.AddUser(user));
    Assert.False(service.AddUser(user));

    User sameUser = new(userId, TimeSpan.FromMilliseconds(10), DateTime.UtcNow);

    Assert.False(service.AddUser(sameUser));
  }
}
