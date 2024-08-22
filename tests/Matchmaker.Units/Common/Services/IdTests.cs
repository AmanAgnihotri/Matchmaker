namespace Matchmaker;

public sealed class IdTests
{
  [Fact]
  public void TestValidityOfGeneratedIds()
  {
    const int capacity = 1000;

    HashSet<long> ids = new(capacity);

    for (int i = 0; i < capacity; i++)
    {
      long id = Id.Create();

      Assert.True(Id.IsValid(id));
      Assert.True(ids.Add(id));
    }
  }
}
