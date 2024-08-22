namespace Matchmaker;

public sealed class UserIdTests
{
  [Fact]
  public void TestNullUserId()
  {
    Assert.Throws<ArgumentNullException>(() => new UserId());
  }

  [Theory]
  [InlineData(0L)]
  [InlineData(-1L)]
  [InlineData(long.MinValue)]
  [InlineData(long.MaxValue)]
  [InlineData(123L)]
  [InlineData(123456789L)]
  [InlineData(100000000000000L)] // 10^14
  [InlineData(9007199254740992)] // 2^53
  public void TestInvalidUserId(long invalidId)
  {
    Assert.False(UserId.TryParse(invalidId, out _));
  }

  [Theory]
  [InlineData(1000000000000000)] // 10^15
  [InlineData(1234567887654321)]
  [InlineData(9007199254740991)] // 2^53 - 1
  public void TestValidUserId(long userId)
  {
    Assert.True(UserId.TryParse(userId, out _));
  }
}
