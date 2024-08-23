namespace Matchmaker.Sessions;

public sealed class MatchmakingServiceTests
{
  [Fact]
  public void TestAddUser()
  {
    Assert.True(UserId.TryParse(Id.Create(), out UserId userId));

    User user = new(userId, TimeSpan.FromMilliseconds(50), DateTime.UtcNow);

    MatchmakingConfig config = new(2, 10, TimeSpan.FromSeconds(4), []);
    MatchmakingState state = new([], []);

    MatchmakingService service = new(config, state);

    Assert.True(service.AddUser(user));
    Assert.False(service.AddUser(user));

    User sameUser = new(userId, TimeSpan.FromMilliseconds(10), DateTime.UtcNow);

    Assert.False(service.AddUser(sameUser));
  }

  [Fact]
  public void TestRemoveUsers()
  {
    Assert.True(UserId.TryParse(Id.Create(), out UserId userId01));
    Assert.True(UserId.TryParse(Id.Create(), out UserId userId02));

    MatchmakingConfig config = new(2, 10, TimeSpan.FromSeconds(4), []);
    MatchmakingState state = new([], []);

    MatchmakingService service = new(config, state);

    User user01 = new(userId01, TimeSpan.FromMilliseconds(80), DateTime.UtcNow);
    User user02 = new(userId02, TimeSpan.FromMilliseconds(30), DateTime.UtcNow);

    Assert.True(service.AddUser(user01));
    Assert.True(service.AddUser(user02));

    Assert.Equal(2, service.GetWaitingUsersCount());

    service.RemoveUsers([user01, user02]);

    Assert.Equal(0, service.GetWaitingUsersCount());
  }

  [Fact]
  public void TestMatchUsersWithOneUser()
  {
    Assert.True(UserId.TryParse(Id.Create(), out UserId userId));

    User user = new(userId, TimeSpan.FromMilliseconds(50), DateTime.UtcNow);

    MatchmakingConfig config = new(2, 10, TimeSpan.FromSeconds(4), [
      new MatchedUsersCountCriterion(0)
    ]);

    MatchmakingState state = new([], []);

    MatchmakingService service = new(config, state);

    service.AddUser(user);

    List<User>? matchedUsers = service.MatchUsers(DateTime.UtcNow);

    Assert.Null(matchedUsers);
  }

  [Fact]
  public void TestMatchUsersWithSimilarLatencies()
  {
    Assert.True(UserId.TryParse(Id.Create(), out UserId userId01));
    Assert.True(UserId.TryParse(Id.Create(), out UserId userId02));
    Assert.True(UserId.TryParse(Id.Create(), out UserId userId03));
    Assert.True(UserId.TryParse(Id.Create(), out UserId userId04));

    DateTime currentTime = DateTime.UtcNow;

    DateTime user01Time = currentTime.AddMilliseconds(-1000);
    DateTime user02Time = currentTime.AddMilliseconds(-900);
    DateTime user03Time = currentTime.AddMilliseconds(-100);
    DateTime user04Time = currentTime.AddMilliseconds(-200);

    User user01 = new(userId01, TimeSpan.FromMilliseconds(80), user01Time);
    User user02 = new(userId02, TimeSpan.FromMilliseconds(30), user02Time);
    User user03 = new(userId03, TimeSpan.FromMilliseconds(20), user03Time);
    User user04 = new(userId04, TimeSpan.FromMilliseconds(90), user04Time);

    MatchmakingConfig config = new(2, 10, TimeSpan.FromSeconds(4), [
      new MatchedUsersCountCriterion(0),
      new LatencyCriterion(TimeSpan.FromMilliseconds(30))
    ]);

    MatchmakingState state = new([], []);

    MatchmakingService service = new(config, state);

    service.AddUser(user01);
    service.AddUser(user02);
    service.AddUser(user03);
    service.AddUser(user04);

    List<User>? matchedUsers01 = service.MatchUsers(currentTime);

    Assert.NotNull(matchedUsers01);

    service.RemoveUsers(matchedUsers01);

    List<User>? matchedUsers02 = service.MatchUsers(currentTime);

    Assert.NotNull(matchedUsers02);

    service.RemoveUsers(matchedUsers02);

    Assert.Equal(2, matchedUsers01.Count);
    Assert.Equal(2, matchedUsers02.Count);

    Assert.Contains(user01, matchedUsers01);
    Assert.Contains(user04, matchedUsers01);

    Assert.Contains(user02, matchedUsers02);
    Assert.Contains(user03, matchedUsers02);
  }
}
