namespace Matchmaker;

public sealed class SessionIdTests
{
  [Fact]
  public void TestNullSessionId()
  {
    Assert.Throws<ArgumentNullException>(() => new SessionId());
  }

  [Fact]
  public void TestValidityOfGeneratedSessionIds()
  {
    const int capacity = 1000;

    HashSet<SessionId> ids = new(capacity);

    for (int i = 0; i < capacity; i++)
    {
      SessionId id = SessionId.Create();

      Assert.True(Id.IsValid(id));
      Assert.True(ids.Add(id));
    }
  }
}
